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
    using System.Reflection;
    using NMock2.Internal;
    using NMock2.Monitoring;

    /// <summary>
    /// The mockery is used to create dynamic mocks and check that all xpectations were met during a unit test.
    /// </summary>
    /// <remarks>Name inspired by Ivan Moore.</remarks>
    public class Mockery : IDisposable
    {
        private static readonly MultiInterfaceFactory facadeFactory = new MultiInterfaceFactory("Mocks");
        private static readonly MockObjectFactory mockObjectFactory = new MockObjectFactory("MockObjects");

        private int depth;
        private IExpectationOrdering expectations;
        private IExpectationOrdering topOrdering;

        /// <summary>
        /// If an unexpected invocation exception is thrown then it is stored here to rethrow it in the 
        /// <see cref="VerifyAllExpectationsHaveBeenMet"/> method - exception cannot be swallowed by tested code.
        /// </summary>
        private ExpectationException thrownUnexpectedInvocationException;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mockery"/> class.
        /// Clears all expectations.
        /// </summary>
        public Mockery()
        {
            this.ClearExpectations();
        }

        /// <summary>
        /// Gets if the following expectations are ordered, i.e. they have to be met in the specified order.
        /// </summary>
        /// <value>Disposable object. When this object is disposed then the ordered expectation mode is set back to the mode it was previously
        /// to call to <see cref="Ordered"/>.</value>
        public IDisposable Ordered
        {
            get 
            { 
                return this.Push(new OrderedExpectations(this.depth)); 
            }
        }

        /// <summary>
        /// Gets if the following expectations are unordered, i.e. they can be met in any order.
        /// </summary>
        /// <value>Disposable object. When this object is disposed then the unordered expectation mode is set back to the mode it was previously
        /// to the call to <see cref="Unordered"/>.</value>
        public IDisposable Unordered
        {
            get 
            { 
                return this.Push(new UnorderedExpectations(this.depth)); 
            }
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type. The type has to be an interface.
        /// </summary>
        /// <param name="mockedType">Type of the dynamic mock. Has to be an interface type.</param>
        /// <returns>A dynamic mock for the specified type.</returns>
        public object NewMock(Type mockedType)
        {
            return this.NewMock(mockedType, this.DefaultNameFor(mockedType));
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type. The type has to be an interface type.
        /// </summary>
        /// <typeparam name="InterfaceOfMock">The type of the interface to mock.</typeparam>
        /// <returns>A dynamic mock for the specified type.</returns>
        public InterfaceOfMock NewMock<InterfaceOfMock>()
        {
            return (InterfaceOfMock)this.NewMock(typeof(InterfaceOfMock));
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type. The type has to be an interface type.
        /// </summary>
        /// <param name="name">The name of the mock. The name is returned as return value of ToString()."/></param>
        /// <typeparam name="InterfaceOfMock">The type of the interface to mock.</typeparam>
        /// <returns>A dynamic mock for the specified type.</returns>
        public InterfaceOfMock NewMock<InterfaceOfMock>(string name)
        {
            return (InterfaceOfMock)this.NewMock(typeof(InterfaceOfMock), name);
        }

        /// <summary>
        /// Creates a new dynamic mock of the specified type. THe type has to be an interface type.
        /// </summary>
        /// <param name="mockedType">Type of the mocked. Has to be an interface type.</param>
        /// <param name="name">The name of the dynamic mock object.</param>
        /// <returns>A named dynamic mock for the specified type.</returns>
        public object NewMock(Type mockedType, string name)
        {
            CheckMockedTypeIsAnInterface(mockedType);
            CheckInterfaceDoesNotContainToStringMethodDeclaration(mockedType);

            Type facadeType = facadeFactory.GetType(typeof(IMockObject), mockedType);
            MockObject mockObject = mockObjectFactory.CreateMockObject(this, mockedType, name);

            ProxyInvokableAdapter adapter =
                new ProxyInvokableAdapter(
                    facadeType,
                    new ProxiedObjectIdentity(
                        mockObject,
                        new Invoker(typeof(IMockObject), mockObject, mockObject)));

            return adapter.GetTransparentProxy();
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
                Exception toBeRethrown = this.thrownUnexpectedInvocationException;
                this.thrownUnexpectedInvocationException = null; // only rethrow once

                throw new ExpectationException(
                    string.Format(
                        "Rethrown unexpected invocation exception. Stack trace of inner exception:{0}{1}",
                        Environment.NewLine,
                        toBeRethrown.StackTrace),
                    toBeRethrown);
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

        internal void AddExpectation(IExpectation expectation)
        {
            this.topOrdering.AddExpectation(expectation);
        }

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

        internal bool TypeHasMethodMatching(Type type, Matcher matcher)
        {
            foreach (Type implementedInterface in this.GetInterfacesImplementedByType(type))
            {
                foreach (MethodInfo method in implementedInterface.GetMethods())
                {
                    if (matcher.Matches(method))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the default name for a type that is used to name mocks.
        /// </summary>
        /// <param name="type">The type to get the default name for.</param>
        /// <returns>Default name for the specified type.</returns>
        protected virtual string DefaultNameFor(Type type)
        {
            string name = type.Name;
            int firstLower = this.FirstLowerCaseChar(name);

            if (firstLower == name.Length)
            {
                return name.ToLower();
            }
            else
            {
                return name.Substring(firstLower - 1, 1).ToLower() + name.Substring(firstLower);
            }
        }

        private static void CheckMockedTypeIsAnInterface(Type mockedType)
        {
            if (!mockedType.IsInterface)
            {
                throw new ArgumentException(
                    string.Format("Classes cannot be mocked. NMock2 supports only mocking of interfaces. You tried to create a mock for type {0}.", mockedType.FullName),
                    "mockedType");
            }
        }

        private static void CheckInterfaceDoesNotContainToStringMethodDeclaration(Type mockedType)
        {
            MethodInfo toString = mockedType.GetMethod("ToString");
            if (toString != null)
            {
                if (toString.GetParameters().Length == 0)
                {
                    throw new ArgumentException("Interfaces must not contain a declaration for ToString().");
                }
            }
        }

        private void ClearExpectations()
        {
            this.depth = 1;
            this.expectations = new UnorderedExpectations();
            this.topOrdering = this.expectations;
        }

        private Type[] GetInterfacesImplementedByType(Type type)
        {
            ArrayList implementedTypes = new ArrayList();
            foreach (Type implementedInterface in type.GetInterfaces())
            {
                implementedTypes.AddRange(this.GetInterfacesImplementedByType(implementedInterface));
            }

            implementedTypes.Add(type);

            Type[] types = new Type[implementedTypes.Count];
            implementedTypes.CopyTo(types);

            return types;
        }

        private Popper Push(IExpectationOrdering newOrdering)
        {
            this.topOrdering.AddExpectation(newOrdering);
            IExpectationOrdering oldOrdering = this.topOrdering;
            this.topOrdering = newOrdering;
            this.depth++;
            return new Popper(this, oldOrdering);
        }

        private void Pop(IExpectationOrdering oldOrdering)
        {
            this.topOrdering = oldOrdering;
            this.depth--;
        }

        private void FailUnmetExpectations()
        {
            DescriptionWriter writer = new DescriptionWriter();
            writer.WriteLine("not all expected invocations were performed");
            this.expectations.DescribeUnmetExpectationsTo(writer);
            this.ClearExpectations();
            throw new ExpectationException(writer.ToString());
        }

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
                if (this.thrownUnexpectedInvocationException == null) // only first exception
                {
                    this.thrownUnexpectedInvocationException = e;
                }

                throw;
            }
        }

        private int FirstLowerCaseChar(string s)
        {
            int i = 0;
            while (i < s.Length && !Char.IsLower(s[i]))
            {
                i++;
            }

            return i;
        }

        private class Popper : IDisposable
        {
            private readonly Mockery mockery;
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

            public void Dispose()
            {
                this.mockery.Pop(this.previous);
            }
        }
    }
}
