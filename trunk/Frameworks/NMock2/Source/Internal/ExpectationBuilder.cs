//-----------------------------------------------------------------------
// <copyright file="ExpectationBuilder.cs" company="NMock2">
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
namespace NMock2.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NMock2.Matchers;
    using NMock2.Syntax;

    public class ExpectationBuilder :
        IReceiverSyntax, IMethodSyntax, IArgumentSyntax, IMatchSyntax, IActionSyntax, ICommentSyntax
    {
        private BuildableExpectation expectation;
        private IMockObject mockObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectationBuilder"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="requiredCountMatcher">The required count matcher.</param>
        /// <param name="acceptedCountMatcher">The accepted count matcher.</param>
        public ExpectationBuilder(string description, Matcher requiredCountMatcher, Matcher acceptedCountMatcher)
        {
            this.expectation = new BuildableExpectation(description, requiredCountMatcher, acceptedCountMatcher);
        }

        public IGetIndexerSyntax Get
        {
            get
            {
                Matcher methodMatcher = NewMethodNameMatcher(string.Empty, "get_Item");
                if (!this.mockObject.HasMethodMatching(methodMatcher))
                {
                    throw new ArgumentException("mock object " + this.mockObject + " does not have an indexed setter");
                }

                this.expectation.DescribeAsIndexer();
                this.expectation.MethodMatcher = methodMatcher;
                return new IndexGetterBuilder(this.expectation, this);
            }
        }

        public ISetIndexerSyntax Set
        {
            get
            {
                Matcher methodMatcher = NewMethodNameMatcher(string.Empty, "set_Item");
                if (!this.mockObject.HasMethodMatching(methodMatcher))
                {
                    throw new ArgumentException("mock object " + this.mockObject + " does not have an indexed setter");
                }

                this.expectation.DescribeAsIndexer();
                this.expectation.MethodMatcher = methodMatcher;
                return new IndexSetterBuilder(this.expectation, this);
            }
        }

        public IMethodSyntax On(object receiver)
        {
            if (receiver is IMockObject)
            {
                this.mockObject = (IMockObject)receiver;

                this.expectation.ReceiverMatcher = new DescriptionOverride(receiver.ToString(), Is.Same(receiver));
                this.mockObject.AddExpectation(this.expectation);
            }
            else
            {
                throw new ArgumentException("not a mock object", "receiver");
            }
            
            return this;
        }
        
        public IArgumentSyntax Method(string methodName, params Type[] typeParams)
        {
            return this.Method(new MethodNameMatcher(methodName), typeParams);
        }

        public IArgumentSyntax Method(MethodInfo method, params Type[] typeParams)
        {
            return this.Method(new DescriptionOverride(method.Name, Is.Same(method)), typeParams);
        }

        public IArgumentSyntax Method(Matcher methodMatcher, params Type[] typeParams)
        {
            if (typeParams != null && typeParams.Length > 0)
            {
                List<Matcher> typeMatchers = new List<Matcher>();
                foreach (Type type in typeParams)
                {
                    typeMatchers.Add(new DescriptionOverride(type.FullName, new SameMatcher(type)));
                }

                return this.Method(
                    methodMatcher, new GenericMethodTypeParametersMatcher(typeMatchers.ToArray()));
            }
            else
            {
                return this.Method(methodMatcher, new AlwaysMatcher(true, string.Empty)); 
            }
        }

        public IArgumentSyntax Method(Matcher methodMatcher, Matcher typeParamsMatcher)
        {
            if (!this.mockObject.HasMethodMatching(methodMatcher))
            {
                throw new ArgumentException("mock object " + this.mockObject + " does not have a method matching " + methodMatcher);
            }

            this.expectation.MethodMatcher = methodMatcher;
            this.expectation.GenericMethodTypeMatcher = typeParamsMatcher;
            
            return this;
        }
        
        public IMatchSyntax GetProperty(string propertyName)
        {
            Matcher methodMatcher = NewMethodNameMatcher(propertyName, "get_" + propertyName);
            if (!this.mockObject.HasMethodMatching(methodMatcher))
            {
                throw new ArgumentException("mock object " + this.mockObject + " does not have a getter for property " + propertyName);
            }

            this.expectation.MethodMatcher = methodMatcher;
            this.expectation.ArgumentsMatcher = new DescriptionOverride(string.Empty, new ArgumentsMatcher());
            
            return this;
        }
        
        public IValueSyntax SetProperty(string propertyName)
        {
            Matcher methodMatcher = NewMethodNameMatcher(propertyName + " = ", "set_" + propertyName);
            if (!this.mockObject.HasMethodMatching(methodMatcher))
            {
                throw new ArgumentException("mock object " + this.mockObject + " does not have a setter for property " + propertyName);
            }

            this.expectation.MethodMatcher = methodMatcher;
            return new PropertyValueBuilder(this);
        }
        
        public IMatchSyntax EventAdd(string eventName)
        {
            return this.EventAdd(eventName, Is.Anything);
        }

        public IMatchSyntax EventAdd(string eventName, Matcher listenerMatcher)
        {
            Matcher methodMatcher = NewMethodNameMatcher(eventName + " += ", "add_" + eventName);
            if (!this.mockObject.HasMethodMatching(methodMatcher))
            {
                throw new ArgumentException("mock object " + this.mockObject + " does not have an event named " + eventName);
            }

            this.expectation.MethodMatcher = methodMatcher;
            this.expectation.ArgumentsMatcher = new ArgumentsMatcher(listenerMatcher);
            return this;
        }
        
        public IMatchSyntax EventAdd(string eventName, Delegate equalListener)
        {
            return this.EventAdd(eventName, Is.EqualTo(equalListener));
        }
        
        public IMatchSyntax EventRemove(string eventName, Matcher listenerMatcher)
        {
            Matcher methodMatcher = NewMethodNameMatcher(eventName + " -= ", "remove_" + eventName);
            if (!this.mockObject.HasMethodMatching(methodMatcher))
            {
                throw new ArgumentException("mock object " + this.mockObject + " does not have an event named " + eventName);
            }

            this.expectation.MethodMatcher = methodMatcher;
            this.expectation.ArgumentsMatcher = new ArgumentsMatcher(listenerMatcher);
            return this;
        }

        public IMatchSyntax EventRemove(string eventName)
        {
            return this.EventRemove(eventName, Is.Anything);
        }

        public IMatchSyntax EventRemove(string eventName, Delegate equalListener)
        {
            return this.EventRemove(eventName, Is.EqualTo(equalListener));
        }
        
        public IMatchSyntax With(params object[] expectedArguments)
        {
            this.expectation.ArgumentsMatcher = new ArgumentsMatcher(ArgumentMatchers(expectedArguments));
            return this;
        }

        public IMatchSyntax WithNoArguments()
        {
            return this.With(new Matcher[0]);
        }
        
        public IMatchSyntax WithAnyArguments()
        {
            this.expectation.ArgumentsMatcher = new AlwaysMatcher(true, "(any arguments)");
            return this;
        }
        
        public IActionSyntax Matching(Matcher matcher)
        {
            this.expectation.AddInvocationMatcher(matcher);
            return this;
        }

        public ICommentSyntax Will(params IAction[] actions)
        {
            foreach (IAction action in actions)
            {
                this.expectation.AddAction(action);
            }

            return this;
        }

        public void Comment(string comment)
        {
            this.expectation.AddComment(comment);
        }

        private static Matcher[] ArgumentMatchers(object[] expectedArguments)
        {
            Matcher[] matchers = new Matcher[expectedArguments.Length];
            for (int i = 0; i < matchers.Length; i++)
            {
                object o = expectedArguments[i];
                matchers[i] = (o is Matcher) ? (Matcher)o : new EqualMatcher(o);
            }

            return matchers;
        }

        private static Matcher NewMethodNameMatcher(string description, string methodName)
        {
            return new DescriptionOverride(description, new MethodNameMatcher(methodName));
        }

        #region Nested Types PropertyValueBuilder, IndexGetterBuilder, IndexSetterBuilder
        private class PropertyValueBuilder : IValueSyntax
        {
            private readonly ExpectationBuilder builder;

            /// <summary>
            /// Initializes a new instance of the <see cref="PropertyValueBuilder"/> class.
            /// </summary>
            /// <param name="builder">The builder.</param>
            public PropertyValueBuilder(ExpectationBuilder builder)
            {
                this.builder = builder;
            }

            public IMatchSyntax To(Matcher valueMatcher)
            {
                return this.builder.With(valueMatcher);
            }

            public IMatchSyntax To(object equalValue)
            {
                return this.To(Is.EqualTo(equalValue));
            }
        }

        private class IndexGetterBuilder : IGetIndexerSyntax
        {
            private readonly BuildableExpectation expectation;
            private readonly ExpectationBuilder builder;

            /// <summary>
            /// Initializes a new instance of the <see cref="IndexGetterBuilder"/> class.
            /// </summary>
            /// <param name="expectation">The expectation.</param>
            /// <param name="builder">The builder.</param>
            public IndexGetterBuilder(BuildableExpectation expectation, ExpectationBuilder builder)
            {
                this.expectation = expectation;
                this.builder = builder;
            }

            public IMatchSyntax this[params object[] expectedArguments]
            {
                get
                {
                    this.expectation.ArgumentsMatcher = new IndexGetterArgumentsMatcher(ArgumentMatchers(expectedArguments));
                    return this.builder;
                }
            }
        }

        private class IndexSetterBuilder : ISetIndexerSyntax, IValueSyntax
        {
            private readonly BuildableExpectation expectation;
            private readonly ExpectationBuilder builder;
            private Matcher[] matchers;

            /// <summary>
            /// Initializes a new instance of the <see cref="IndexSetterBuilder"/> class.
            /// </summary>
            /// <param name="expectation">The expectation.</param>
            /// <param name="builder">The builder.</param>
            public IndexSetterBuilder(BuildableExpectation expectation, ExpectationBuilder builder)
            {
                this.expectation = expectation;
                this.builder = builder;
            }

            public IValueSyntax this[params object[] expectedArguments]
            {
                get
                {
                    Matcher[] indexMatchers = ArgumentMatchers(expectedArguments);
                    this.matchers = new Matcher[indexMatchers.Length + 1];
                    Array.Copy(indexMatchers, this.matchers, indexMatchers.Length);
                    this.SetValueMatcher(Is.Anything);
                    return this;
                }
            }

            public IMatchSyntax To(Matcher matcher)
            {
                this.SetValueMatcher(matcher);
                return this.builder;
            }

            public IMatchSyntax To(object equalValue)
            {
                return this.To(Is.EqualTo(equalValue));
            }

            private void SetValueMatcher(Matcher matcher)
            {
                this.matchers[this.matchers.Length - 1] = matcher;
                this.expectation.ArgumentsMatcher = new IndexSetterArgumentsMatcher(this.matchers);
            }
        }
        #endregion
    }
}
