//-----------------------------------------------------------------------
// <copyright file="IReceiverSyntax.cs" company="NMock2">
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
namespace NMock2.Syntax
{
    /// <summary>
    /// Syntax defining a receiver.
    /// </summary>
    public interface IReceiverSyntax
    {
        /// <summary>
        /// Defines the receiver.
        /// </summary>
        /// <param name="o">The dynamic mock on which the expectation or stub is applied.</param>
        /// <returns>Method syntax defining the method, property or event.</returns>
        IMethodSyntax On(object o);
    }
}
