//-----------------------------------------------------------------------
// <copyright file="Mockery.cs" company="NMock2">
//
//   http://www.sourceforge.net/projects/NMock2
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace NMock2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Internal;
    using Monitoring;
    using NMock2.Matchers;

    /// <summary>
    /// The mockery is used to create dynamic mocks and check that all expectations were met during a unit test.
    /// </summary>
    /// <remarks>Name inspired by Ivan Moore.</remarks>
    public class Mockery : IDisposable
    {
        /// <summary>
        /// In the rare case where the default mock object factory is replaced, we hold on to the
        /// previous factory (or factories) in case we need to switch back to them.
        /// </summary>
        private static readonly Dictionary<Type, IMockObjectFactory> availableMockObjectFactories = new Dictionary<Type, IMockObjectFactory>();

        /// <summary>
        /// The mock object factory that is being used by this Mockery instance.
        /// </summary>
        private readonly IMockObjectFactory currentMockObjectFactory;

        /// <summary>
        /// The mock object factory that will be used when a new Mockery instance is created
        /// </summary>
        private static IMockObjectFactory defaultMockObjectFactory;

        /// <summary>
        /// Depth of cascaded ordered, unordered expecation blocks.
        /// </summary>
        private int depth;

        /// <summary>
        /// All expectations.
        /// </summary>
        private IExpectationOrdering expectations;

        /// <summary>
        /// Expectations at current cascade level.
        /// </summary>
        private IExpectationOrdering topOrdering;

        /// <summary>
        /// If an unexpected invocation exception is thrown then it is stored here to rethrow it in the 
        /// <see cref="VerifyAllExpectationsHaveBeenMet"/> method - exception cannot be swallowed by tested code.
        /// </summary>
        private ExpectationException thrownUnexpectedInvocationException;

        /// <summary>
        /// Initializes static members of the <see cref="Mockery"/> class.
        /// </summary>
        static Mockery()
        {
            ChangeDefaultMockObjectFactory(typeof(CastleMockObjectFactory));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mockery"/> class.
        /// Clears all expectations.
        /// </summary>
        public Mockery()
        {
            this.currentMockObjectFactory = defaultMockObjectFactory;
            
            this.ClearExpectations();
        }

        /// <summary>
        /// Gets a disposabale object and tells the mockery that the following expectations are ordered, i.e. they have to be met in the specified order.
        /// Dispose the returned value to return to previous mode.
        /// </summary>
        /// <value>Disposable object. When this object is disposed then the ordered expectation mode is set back to the mode it was previously
        /// to call to <see cref="Ordered"/>.</value>
        public IDisposable Ordered
        {
            get { return this.Push(new OrderedExpectations(this.depth)); }
        }

        /// <summary>
        /// Gets a disposabale object and tells the mockery that the following expectations are unordered, i.e. they can be met in any order.
        /// Dispose the returned value to return to previous mode.
        /// </summary>
        /// <value>Disposable object. When this object is disposed then the unordered expectation mode is set back to the mode it was previously
        /// to the call to <see cref="Unordered"/>.</value>
        public IDisposable Unordered
        {
            get { return this.Push(new UnorderedExpectations(this.depth)); }
        }

        /// <summary>
        /// Allows the default IMockObjectFactory to be replaced with a different implementation.
        /// </summary>
        /// <param name="factoryType">The System.Type of the IMockObjectFactory implementation to use.
        /// This is expected to implement IMockObjectFactory and have a default constructor.</param>
        public static void ChangeDefaultMockObjectFactory(Type factoryType)
        {
            if (!typeof(IMockObjectFactory).IsAssignableFrom(factoryType))
            {
                throw new ArgumentException("Supplied factory type does not implement IMockObjectFactory", "factoryType");
            }

            lock (availableMockObjectFactories)
            {
                if (!availableMockObjectFactories.TryGetValue(factoryType, out defaultMockObjectFactory))
                {
                    try
                    {
                        defaultMockObjectFactory = (IMockObjectFactory)Activator.CreateInstance(factoryType);
                        availableMockObjectFactories[factoryType] = defaultMockObjectFactory;
                    }
                    catch (MissingMethodException)
                    {
                        throw new ArgumentException("Supplied factory type does not have a default constructor", "factoryType");
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type.
        /// </summary>
        /// <param name="mockedType">The type to mock.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A dynamic mock for the specified type.</returns>
        public object NewMock(Type mockedType, params object[] constructorArgs)
        {
            return this.CreateMockForType(mockedType, null, MockStyle.Default, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type.
        /// </summary>
        /// <param name="mockedType">The type to mock.</param>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named dynamic mock for the specified type.</returns>
        public object NewMock(Type mockedType, MockStyle mockStyle, params object[] constructorArgs)
        {
            return this.CreateMockForType(mockedType, null, mockStyle, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type(s).
        /// </summary>
        /// <param name="mockedType">The type to mock.</param>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="additionalTypesToMock">Any additional types to be mocked. Between this and
        /// the mockedType parameter, there can be at most one class type specified. All other types
        /// must be interfaces.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named dynamic mock for the specified type(s).</returns>
        public object NewMock(Type mockedType, MockStyle mockStyle, Type[] additionalTypesToMock, params object[] constructorArgs)
        {
            return this.CreateMockForType(mockedType, null, mockStyle, additionalTypesToMock, constructorArgs);
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type.
        /// </summary>
        /// <typeparam name="MockedType">The type to mock.</typeparam>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A dynamic mock for the specified type.</returns>
        public MockedType NewMock<MockedType>(params object[] constructorArgs)
        {
            return (MockedType)this.CreateMockForType(typeof(MockedType), null, MockStyle.Default, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type.
        /// </summary>
        /// <typeparam name="MockedType">The type to mock.</typeparam>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A dynamic mock for the specified type.</returns>
        public MockedType NewMock<MockedType>(MockStyle mockStyle, params object[] constructorArgs)
        {
            return (MockedType)this.CreateMockForType(typeof(MockedType), null, mockStyle, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type(s).
        /// </summary>
        /// <typeparam name="MockedType">The type to mock.</typeparam>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="additionalTypesToMock">Any additional types to be mocked. Between this and
        /// the MockedType type parameter, there can be at most one class type specified. All other types
        /// must be interfaces.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A dynamic mock for the specified type(s).</returns>
        public MockedType NewMock<MockedType>(MockStyle mockStyle, Type[] additionalTypesToMock, params object[] constructorArgs)
        {
            return (MockedType)this.CreateMockForType(typeof(MockedType), null, mockStyle, additionalTypesToMock, constructorArgs);
        }

        /// <summary>
        /// Creates a new named dynamic mock of the specified type.
        /// </summary>
        /// <param name="mockedType">The type to mock.</param>
        /// <param name="name">A name for the mock that will be used in error messages.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named mock.</returns>
        public object NewNamedMock(Type mockedType, string name, params object[] constructorArgs)
        {
            return this.CreateMockForType(mockedType, name, MockStyle.Default, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new named dynamic mock of the specified type and allows the style
        /// of the mock to be specified.
        /// </summary>
        /// <param name="mockedType">The type to mock.</param>
        /// <param name="name">A name for the mock that will be used in error messages.</param>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named mock.</returns>
        public object NewNamedMock(Type mockedType, string name, MockStyle mockStyle, params object[] constructorArgs)
        {
            return this.CreateMockForType(mockedType, name, mockStyle, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new named dynamic mock of the specified type(s) and allows the style
        /// of the mock to be specified.
        /// </summary>
        /// <param name="mockedType">The type to mock.</param>
        /// <param name="name">A name for the mock that will be used in error messages.</param>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="additionalTypesToMock">Any additional types to be mocked. Between this and
        /// the mockedType parameter, there can be at most one class type specified. All other types
        /// must be interfaces.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named mock.</returns>
        public object NewNamedMock(Type mockedType, string name, MockStyle mockStyle, Type[] additionalTypesToMock, params object[] constructorArgs)
        {
            return this.CreateMockForType(mockedType, name, mockStyle, additionalTypesToMock, constructorArgs);
        }

        /// <summary>
        /// Creates a new named dynamic mock of the specified type.
        /// </summary>
        /// <typeparam name="MockedType">The type to mock.</typeparam>
        /// <param name="name">A name for the mock that will be used in error messages.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named mock.</returns>
        public MockedType NewNamedMock<MockedType>(string name, params object[] constructorArgs)
        {
            return (MockedType)this.CreateMockForType(typeof(MockedType), name, MockStyle.Default, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new named dynamic mock of the specified type and allows the style
        /// of the mock to be specified.
        /// </summary>
        /// <typeparam name="MockedType">The type to mock.</typeparam>
        /// <param name="name">A name for the mock that will be used in error messages.</param>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named mock.</returns>
        public MockedType NewNamedMock<MockedType>(string name, MockStyle mockStyle, params object[] constructorArgs)
        {
            return (MockedType)this.CreateMockForType(typeof(MockedType), name, mockStyle, new Type[0], constructorArgs);
        }

        /// <summary>
        /// Creates a new named dynamic mock of the specified type(s) and allows the style
        /// of the mock to be specified.
        /// </summary>
        /// <typeparam name="MockedType">The type to mock.</typeparam>
        /// <param name="name">A name for the mock that will be used in error messages.</param>
        /// <param name="mockStyle">Specifies how the mock object should behave when first created.</param>
        /// <param name="additionalTypesToMock">Any additional types to be mocked. Between this and
        /// the MockedType type parameter, there can be at most one class type specified. All other types
        /// must be interfaces.</param>
        /// <param name="constructorArgs">The arguments for the constructor of the class to be mocked.
        /// Only applicable when mocking classes with non-default constructors.</param>
        /// <returns>A named mock.</returns>
        public MockedType NewNamedMock<MockedType>(string name, MockStyle mockStyle, Type[] additionalTypesToMock, params object[] constructorArgs)
        {
            return (MockedType)this.CreateMockForType(typeof(MockedType), null, mockStyle, additionalTypesToMock, constructorArgs);
        }

        /// <summary>
        /// Verifies that all expectations have been met.
        /// Will be called in <see cref="Dispose"/>, too. 
        /// </summary>
        public void VerifyAllExpectationsHaveBeenMet()
        {
            // check for swallowed exception
            if (this.thrownUnexpectedInvocationException != null)
            {
                Exception exceptionToBeRethrown = this.thrownUnexpectedInvocationException;
                this.thrownUnexpectedInvocationException = null; // only rethrow once

                throw new ExpectationException(
                    string.Format(
                        "Rethrown unexpected invocation exception. Stack trace of inner exception:{0}{1}",
                        Environment.NewLine,
                        exceptionToBeRethrown.StackTrace),
                    exceptionToBeRethrown);
            }

            if (!this.expectations.HasBeenMet)
            {
                this.FailUnmetExpectations();
            }
        }

        /// <summary>
        /// Disposes the mockery be verifying that all expectations were met.
        /// </summary>
        public void Dispose()
        {
            this.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Adds the expectation.
        /// </summary>
        /// <param name="expectation">The expectation.</param>
        internal void AddExpectation(IExpectation expectation)
        {
            this.topOrdering.AddExpectation(expectation);
        }
        
        public delegate object ResolveTypeDelegate(object mock, Type requestedType);

        private ResolveTypeDelegate resolveTypeDelegate;

        /// <summary>
        /// Sets the resolve type handler used to override default values returned by stubs.
        /// </summary>
        /// <param name="resolveTypeHandler">The resolve type handler.</param>
        public void SetResolveTypeHandler(ResolveTypeDelegate resolveTypeHandler)
        {
            this.resolveTypeDelegate = resolveTypeHandler;
        }

        internal object ResolveType(object mock, Type requestedType)
        {
            return this.resolveTypeDelegate != null ? this.resolveTypeDelegate(mock, requestedType) : Missing.Value;
        }

        public delegate MockStyle? ResolveMockStyle(object mock, Type requestedType);

        private ResolveMockStyle mockStyleResolver;

        /// <summary>
        /// Sets the mock style resolver used to override the default mock style used when mocks are returned by stubs.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public void SetMockStyleResolver(ResolveMockStyle resolver)
        {
            this.mockStyleResolver = resolver;
        }

        internal MockStyle? GetDependencyMockStyle(object mock, Type requestedType)
        {
            return this.mockStyleResolver != null ? this.mockStyleResolver(mock, requestedType) : null;
        }
        
        /// <summary>
        /// Dispatches the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        internal void Dispatch(Invocation invocation)
        {
            if (this.expectations.Matches(invocation))
            {
                this.expectations.Perform(invocation);
            }
            else
            {
                this.FailUnexpectedInvocation(invocation);
            }
        }

        /// <summary>
        /// Determines whether there exist expectations for the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns><c>true</c> if there exist expectations for the specified invocation; otherwise, <c>false</c>.
        /// </returns>
        internal bool HasExpectationFor(Invocation invocation)
        {
            return this.expectations.Matches(invocation);
        }

        /// <summary>
        /// Returns the default name for a type that is used to name mocks.
        /// </summary>
        /// <param name="type">The type to get the default name for.</param>
        /// <returns>Default name for the specified type.</returns>
        protected virtual string DefaultNameFor(Type type)
        {
            string name = type.Name;
            int firstLower = FirstLowerCaseChar(name);

            return 
                firstLower == name.Length ? 
                name.ToLower() : 
                name.Substring(firstLower - 1, 1).ToLower() + name.Substring(firstLower);
        }

        /// <summary>
        /// Finds the first lower case char in the specified string.
        /// </summary>
        /// <param name="s">The string to inspect.</param>
        /// <returns>the first lower case char in the specified string.</returns>
        private static int FirstLowerCaseChar(string s)
        {
            int i = 0;
            while (i < s.Length && !Char.IsLower(s[i]))
            {
                i++;
            }

            return i;
        }

        /// <summary>
        /// Checks that interfaces do not contain ToString method declarations.
        /// </summary>
        /// <param name="mockedTypes">The types that are to be mocked.</param>
        private static void CheckInterfacesDoNotContainToStringMethodDeclaration(CompositeType mockedTypes)
        {
            foreach (MethodInfo method in mockedTypes.GetMatchingMethods(new MethodNameMatcher("ToString"), false))
            {
                if (method.ReflectedType.IsInterface && method.GetParameters().Length == 0)
                {
                    throw new ArgumentException("Interfaces must not contain a declaration for ToString().");
                }
            }
        }

        /// <summary>
        /// Clears the expectations.
        /// </summary>
        private void ClearExpectations()
        {
            this.depth = 1;
            this.expectations = new UnorderedExpectations();
            this.topOrdering = this.expectations;
        }

        /// <summary>
        /// Creates a new mock for the specified type.
        /// </summary>
        /// <param name="mockedType">Type of the mock.</param>
        /// <param name="name">The name of the mock.</param>
        /// <param name="mockStyle">The mock style.</param>
        /// <param name="additionalTypesToMock">Any additional interfaces that should also be mocked.</param>
        /// <param name="constructorArgs">The constructor arguments.</param>
        /// <returns>A newly created mock for type</returns>
        private object CreateMockForType(Type mockedType, string name, MockStyle mockStyle, Type[] additionalTypesToMock, params object[] constructorArgs)
        {
            if (name == null)
            {
                name = this.DefaultNameFor(mockedType);
            }

            CompositeType compositeType = new CompositeType(mockedType, additionalTypesToMock);

            if (compositeType.PrimaryType.IsInterface)
            {
                if (constructorArgs.Length > 0)
                {
                    throw new InvalidOperationException("Cannot specify constructor arguments when mocking an interface");
                }

                CheckInterfacesDoNotContainToStringMethodDeclaration(compositeType);
            }

            return this.currentMockObjectFactory.CreateMock(this, compositeType, name, mockStyle, constructorArgs);

            //throw new ArgumentException("Can only mock classes and interfaces", "mockedType"); // TODO: More error checking
        }

        /// <summary>
        /// Pushes the specified new ordering on the expectations stack.
        /// </summary>
        /// <param name="newOrdering">The new ordering.</param>
        /// <returns>Disposable popper.</returns>
        private Popper Push(IExpectationOrdering newOrdering)
        {
            this.topOrdering.AddExpectation(newOrdering);
            IExpectationOrdering oldOrdering = this.topOrdering;
            this.topOrdering = newOrdering;
            this.depth++;
            return new Popper(this, oldOrdering);
        }

        /// <summary>
        /// Pops the specified old ordering from the expectations stack.
        /// </summary>
        /// <param name="oldOrdering">The old ordering.</param>
        private void Pop(IExpectationOrdering oldOrdering)
        {
            this.topOrdering = oldOrdering;
            this.depth--;
        }

        /// <summary>
        /// Throws an exception listing all unmet expectations.
        /// </summary>
        private void FailUnmetExpectations()
        {
            DescriptionWriter writer = new DescriptionWriter();
            writer.WriteLine("not all expected invocations were performed");
            this.expectations.DescribeUnmetExpectationsTo(writer);
            this.ClearExpectations();

            throw new ExpectationException(writer.ToString());
        }

        /// <summary>
        /// Throws an exception indicating that the specified invocation is not expected.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        private void FailUnexpectedInvocation(Invocation invocation)
        {
            DescriptionWriter writer = new DescriptionWriter();
            writer.Write("unexpected invocation of ");
            invocation.DescribeTo(writer);
            writer.WriteLine();
            this.expectations.DescribeActiveExpectationsTo(writer);

            // try catch to get exception with stack trace.
            try
            {
                throw new ExpectationException(writer.ToString());
            }
            catch (ExpectationException e)
            {
                // remember only first exception
                if (this.thrownUnexpectedInvocationException == null)
                {
                    this.thrownUnexpectedInvocationException = e;
                }

                throw;
            }
        }

        /// <summary>
        /// A popper pops an expectation ordering from the expectations stack on disposal.
        /// </summary>
        private class Popper : IDisposable
        {
            /// <summary>
            /// The mockery.
            /// </summary>
            private readonly Mockery mockery;

            /// <summary>
            /// The previous expectaion ordering.
            /// </summary>
            private readonly IExpectationOrdering previous;

            /// <summary>
            /// Initializes a new instance of the <see cref="Popper"/> class.
            /// </summary>
            /// <param name="mockery">The mockery.</param>
            /// <param name="previous">The previous.</param>
            public Popper(Mockery mockery, IExpectationOrdering previous)
            {
                this.previous = previous;
                this.mockery = mockery;
            }

            /// <summary>
            /// Pops the expectation ordering from the stack.
            /// </summary>
            public void Dispose()
            {
                this.mockery.Pop(this.previous);
            }
        }
    }
}
