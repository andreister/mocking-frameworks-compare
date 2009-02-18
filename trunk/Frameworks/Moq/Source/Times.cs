﻿//Copyright (c) 2007, Moq Team 
//http://code.google.com/p/moq/
//All rights reserved.

//Redistribution and use in source and binary forms, 
//with or without modification, are permitted provided 
//that the following conditions are met:

//    * Redistributions of source code must retain the 
//    above copyright notice, this list of conditions and 
//    the following disclaimer.

//    * Redistributions in binary form must reproduce 
//    the above copyright notice, this list of conditions 
//    and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution.

//    * Neither the name of the Moq Team nor the 
//    names of its contributors may be used to endorse 
//    or promote products derived from this software 
//    without specific prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
//CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
//MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
//BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
//INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
//SUCH DAMAGE.

//[This is the BSD license, see
// http://www.opensource.org/licenses/bsd-license.php]

using System.Globalization;
using Moq.Properties;
using System;

namespace Moq
{
	/// <summary>
	/// Defines the number of invocations allowed by a mocked method.
	/// </summary>
	public class Times
	{
		private Func<int, bool> evaluator;
		private string messageFormat;
		private int from;
		private int to;

		private Times(Func<int, bool> evaluator, int from, int to, string messageFormat)
		{
			this.evaluator = evaluator;
			this.from = from;
			this.to = to;
			this.messageFormat = messageFormat;
		}

		/// <summary>
		/// Specifies that a mocked method should be invoked <paramref name="times"/> times as minimum.
		/// </summary>
		/// <param name="callCount">The minimun number of times.</param>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times AtLeast(int callCount)
		{
			Guard.ArgumentNotOutOfRangeInclusive(callCount, 1, int.MaxValue, "callCount");

			return new Times(c => c >= callCount, callCount, int.MaxValue, Resources.NoMatchingCallsAtLeast);
		}

		/// <summary>
		/// Specifies that a mocked method should be invoked one time as minimum.
		/// </summary>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times AtLeastOnce()
		{
			return new Times(c => c >= 1, 1, int.MaxValue, Resources.NoMatchingCallsAtLeastOnce);
		}

		/// <summary>
		/// Specifies that a mocked method should be invoked <paramref name="times"/> time as maximun.
		/// </summary>
		/// <param name="callCount">The maximun number of times.</param>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times AtMost(int callCount)
		{
			Guard.ArgumentNotOutOfRangeInclusive(callCount, 0, int.MaxValue, "callCount");

			return new Times(c => c >= 0 && c <= callCount, 0, callCount, Resources.NoMatchingCallsAtMost);
		}

		/// <summary>
		/// Specifies that a mocked method should be invoked one time as maximun.
		/// </summary>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times AtMostOnce()
		{
			return new Times(c => c >= 0 && c <= 1, 0, 1, Resources.NoMatchingCallsAtMostOnce);
		}

		/// <summary>
		/// Specifies that a mocked method should be invoked between <paramref name="from"/> and
		/// <paramref name="to"/> times.
		/// </summary>
		/// <param name="callCountFrom">The minimun number of times.</param>
		/// <param name="callCountTo">The maximun number of times.</param>
		/// <param name="rangeKind">The kind of range. See <see cref="Range"/>.</param>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times Between(int callCountFrom, int callCountTo, Range rangeKind)
		{
			if (rangeKind == Range.Exclusive)
			{
				Guard.ArgumentNotOutOfRangeExclusive(callCountFrom, 0, callCountTo, "callCountFrom");
				if (callCountTo - callCountFrom == 1)
				{
					throw new ArgumentOutOfRangeException("callCountTo");
				}

				return new Times(
					c => c > callCountFrom && c < callCountTo,
					callCountFrom,
					callCountTo,
					Resources.NoMatchingCallsBetweenExclusive);
			}

			Guard.ArgumentNotOutOfRangeInclusive(callCountFrom, 0, callCountTo, "callCountFrom");
			return new Times(
				c => c >= callCountFrom && c <= callCountTo,
				callCountFrom,
				callCountTo,
				Resources.NoMatchingCallsBetweenInclusive);
		}

		/// <summary>
		/// Specifies that a mocked method should be invoked exactly <paramref name="times"/> times.
		/// </summary>
		/// <param name="callCount">The times that a method or property can be called.</param>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times Exactly(int callCount)
		{
			Guard.ArgumentNotOutOfRangeInclusive(callCount, 0, int.MaxValue, "callCount");

			return new Times(c => c == callCount, callCount, callCount, Resources.NoMatchingCallsExactly);
		}

		/// <summary>
		/// Specifies that a mocked method should not be invoked.
		/// </summary>
		/// <returns>An object defining the allowed number of invocations.</returns>
		public static Times Never()
		{
			return new Times(c => c == 0, 0, 0, Resources.NoMatchingCallsNever);
		}

		internal string GetExceptionMessage(string failMessage, string expression)
		{
			return string.Format(
				CultureInfo.CurrentCulture,
				this.messageFormat,
				failMessage,
				expression,
				this.from,
				this.to);
		}

		internal bool Verify(int callCount)
		{
			return this.evaluator(callCount);
		}
	}
}