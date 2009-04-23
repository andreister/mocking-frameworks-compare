//-----------------------------------------------------------------------
// <copyright file="Verify.cs" company="NMock2">
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
    using NMock2.Internal;

    /// <summary>
    /// Verify that a condition is met.
    /// </summary>
    public class Verify
    {
        /// <summary>
        /// Verifies that the <paramref name="actualValue"/> is matched by the <paramref name="matcher"/>.
        /// </summary>
        /// <param name="actualValue">The actual value to match.</param>
        /// <param name="matcher">The matcher.</param>
        /// <param name="message">The error message.</param>
        /// <param name="formatArgs">The format args for the error message.</param>
        /// <exception cref="ExpectationException">Thrown if value does not match.</exception>
        public static void That(object actualValue, Matcher matcher, string message, params object[] formatArgs)
        {
            if (!matcher.Matches(actualValue))
            {
                DescriptionWriter writer = new DescriptionWriter();
                writer.Write(message, formatArgs);
                WriteDescriptionOfFailedMatch(writer, actualValue, matcher);

                throw new ExpectationException(writer.ToString());
            }
        }

        /// <summary>
        /// Verifies that the <paramref name="actualValue"/> is matched by the <paramref name="matcher"/>.
        /// </summary>
        /// <param name="actualValue">The actual value.</param>
        /// <param name="matcher">The matcher.</param>
        /// <exception cref="ExpectationException">Thrown if value does not match.</exception>
        public static void That(object actualValue, Matcher matcher)
        {
            if (!matcher.Matches(actualValue))
            {
                DescriptionWriter writer = new DescriptionWriter();
                WriteDescriptionOfFailedMatch(writer, actualValue, matcher);
                
                throw new ExpectationException(writer.ToString());
            }
        }
        
        private static void WriteDescriptionOfFailedMatch(DescriptionWriter writer, object actualValue, Matcher matcher)
        {
            writer.WriteLine();
            writer.Write("Expected: ");
            matcher.DescribeTo(writer);
            writer.WriteLine();
            writer.Write("Actual:   ");
            writer.Write(actualValue);
        }
    }
}