//-----------------------------------------------------------------------
// <copyright file="IExpectation.cs" company="NMock2">
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
    using System.Collections.Generic;
    using System.IO;
    using Internal;
    using NMock2.Monitoring;

    /// <summary>
    /// Represents an expectation.
    /// </summary>
    public interface IExpectation
    {
        bool IsActive { get; }

        bool HasBeenMet { get; }

        bool Matches(Invocation invocation);

        void Perform(Invocation invocation);

        void DescribeActiveExpectationsTo(TextWriter writer);

        void DescribeUnmetExpectationsTo(TextWriter writer);

        /// <summary>
        /// Adds all expectations to <paramref name="result"/> that are associated to <paramref name="mock"/>.
        /// </summary>
        /// <param name="mock">The mock for which expectations are queried.</param>
        /// <param name="result">The result to add matching expectations to.</param>
        void QueryExpectationsBelongingTo(IMockObject mock, IList<IExpectation> result);
    }
}

