//-----------------------------------------------------------------------
// <copyright file="IMockObject.cs" company="NMock2">
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

    /// <summary>
    /// Interface for mocks.
    /// </summary>
    public interface IMockObject
    {
        bool HasMethodMatching(Matcher methodMatcher);

        /// <summary>
        /// Adds an expectation to this mock.
        /// </summary>
        /// <param name="expectation">The expectation to add.</param>
        void AddExpectation(IExpectation expectation);

        /// <summary>
        /// Adds an event handler on this mock.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The handler to add.</param>
        void AddEventHandler(string eventName, Delegate handler);

        /// <summary>
        /// Removes an event handler from this mock.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The handler to remove.</param>
        void RemoveEventHandler(string eventName, Delegate handler);

        /// <summary>
        /// Raises an event on this mock.
        /// </summary>
        /// <param name="eventName">Name of the event to fire.</param>
        /// <param name="args">The arguments passed to the event.</param>
        void RaiseEvent(string eventName, params object[] args);
    }
}
