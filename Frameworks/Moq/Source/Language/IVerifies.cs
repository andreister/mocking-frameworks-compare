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

using System.ComponentModel;

namespace Moq.Language
{
	/// <summary>
	/// Defines the <c>Verifiable</c> verb.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IVerifies : IHideObjectMembers
	{ 
		/// <summary>
		/// Marks the expectation as verifiable, meaning that a call 
		/// to <see cref="Mock.Verify()"/> will check if this particular 
		/// expectation was met.
		/// </summary>
		/// <example>
		/// The following example marks the expectation as verifiable:
		/// <code>
		/// mock.Expect(x => x.Execute("ping"))
		///     .Returns(true)
		///     .Verifiable();
		/// </code>
		/// </example>
		void Verifiable();

		/// <summary>
		/// Marks the expectation as verifiable, meaning that a call 
		/// to <see cref="Mock.Verify()"/> will check if this particular 
		/// expectation was met, and specifies a message for failures.
		/// </summary>
		/// <example>
		/// The following example marks the expectation as verifiable:
		/// <code>
		/// mock.Expect(x => x.Execute("ping"))
		///     .Returns(true)
		///     .Verifiable("Ping should be executed always!");
		/// </code>
		/// </example>
		void Verifiable(string failMessage);
	}
}
