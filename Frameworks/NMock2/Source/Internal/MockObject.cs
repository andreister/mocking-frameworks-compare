//-----------------------------------------------------------------------
// <copyright file="MockObject.cs" company="NMock2">
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
    using System.Collections;
    using System.Reflection;
    using NMock2.Monitoring;

    public class MockObject : IInvokable, IMockObject
    {
        /// <summary>
        /// Stores the backlink to the mockery which created this mock object.
        /// </summary>
        private readonly Mockery mockery;

        /// <summary>
        /// Stores the mocked type.
        /// </summary>
        private readonly Type mockedType;

        /// <summary>
        /// Stores the name of the mock object.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Stores the event handlers that could be added to the mock object.
        /// </summary>
        private readonly IDictionary eventHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockObject"/> class.
        /// </summary>
        /// <param name="mockery">The mockery.</param>
        /// <param name="mockedType">Type of the mocked object.</param>
        /// <param name="name">The name of the mock object.</param>
        protected MockObject(Mockery mockery, Type mockedType, string name)
        {
            this.mockery = mockery;
            this.mockedType = mockedType;
            this.name = name;
            this.eventHandlers = new Hashtable();
        }

        public override string ToString()
        {
            return this.name;
        }

        public void Invoke(Invocation invocation)
        {
            this.mockery.Dispatch(invocation);
        }

        public bool HasMethodMatching(Matcher methodMatcher)
        {
            return this.mockery.TypeHasMethodMatching(this.mockedType, methodMatcher);
        }

        public void AddExpectation(IExpectation expectation)
        {
            this.mockery.AddExpectation(expectation);
        }

        public void AddEventHandler(string eventName, Delegate handler)
        {
            ArrayList handlers = (ArrayList)this.eventHandlers[eventName];

            if (handlers == null)
            {
                handlers = new ArrayList();
                this.eventHandlers.Add(eventName, handlers);
            }

            if (! handlers.Contains(handler)) 
            { 
                handlers.Add(handler); 
            }
        }

        public void RaiseEvent(string eventName, params object[] args)
        {
            IEnumerable handlers = (IEnumerable)this.eventHandlers[eventName];
    
            if (handlers != null)
            {
                foreach (Delegate d in handlers)
                {
                    try
                    {
                        d.DynamicInvoke(args);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
            }
            else
            {
                // check whether the mocked type has this event
                if (this.mockedType.GetEvent(eventName) == null)
                {
                    throw new ArgumentException(
                        string.Format("Event with name {0} does not exist on type {1}.", eventName, this.mockedType.FullName),
                        "eventName");
                }
            }
        }

        public void RemoveEventHandler(string eventName, Delegate handler)
        {
            ArrayList handlers = (ArrayList)this.eventHandlers[eventName];

            if (handlers != null)
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    this.eventHandlers.Remove(eventName);
                }
            }
        }
    }
}
