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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq.Proxy;

namespace Moq
{
	/// <summary>
	/// Implements the actual interception and method invocation for 
	/// all mocks.
	/// </summary>
	internal class Interceptor : ICallInterceptor
	{
		private MockBehavior behavior;
		private Type targetType;
		private Dictionary<ExpressionKey, IProxyCall> calls = new Dictionary<ExpressionKey, IProxyCall>();
		private List<IProxyCall> orderedCalls = new List<IProxyCall>();
		private List<ICallContext> actualInvocations = new List<ICallContext>();

		public Interceptor(MockBehavior behavior, Type targetType, Mock mock)
		{
			this.behavior = behavior;
			this.targetType = targetType;
			this.Mock = mock;
		}

		internal IEnumerable<ICallContext> ActualCalls
		{
			get { return this.actualInvocations; }
		}

		internal Mock Mock { get; private set; }

		internal IEnumerable<IProxyCall> OrderedCalls
		{
			get { return this.orderedCalls; }
		}

		internal void Verify()
		{
			// The IsNever case would have thrown the moment the member is invoked, 
			// so we can safely skip here all such setups.
			VerifyOrThrow(call => call.IsVerifiable && !call.IsNever && !call.Invoked);
		}

		internal void VerifyAll()
		{
			// The IsNever case would have thrown the moment the member is invoked, 
			// so we can safely skip here all such setups.
			VerifyOrThrow(call => !call.IsNever && !call.Invoked);
		}

		private void VerifyOrThrow(Func<IProxyCall, bool> match)
		{
			var failures = calls.Values.Where(match).ToList();

			if (failures.Count > 0)
			{
				throw new MockVerificationException(failures);
			}
		}

		public void AddCall(IProxyCall call, SetupKind kind)
		{
			var expr = call.SetupExpression.PartialMatcherAwareEval();

			var s = expr.ToStringFixed(true);

			if (kind == SetupKind.PropertySet)
			{
				s = "set::" + s;
			}

			var constants = new ConstantsVisitor(expr).Values;
			var key = new ExpressionKey(s, constants);

			if (calls.ContainsKey(key))
			{
				// Remove previous from ordered calls
				orderedCalls.Remove(calls[key]);
			}

			calls[key] = call;
			orderedCalls.Add(call);

			//if (kind == ExpectKind.PropertySet)
			//    calls["set::" + expr.ToStringFixed()] = call;
			//else
			//    calls[expr.ToStringFixed()] = call;
		}

		public void Intercept(ICallContext invocation)
		{
			// Track current invocation if we're in "record" mode in a fluent invocation context.
			if (FluentMockContext.IsActive)
			{
				FluentMockContext.Current.Add(this.Mock, invocation);
			}

			// TODO: too many ifs in this method.
			// see how to refactor with strategies.
			if (invocation.Method.DeclaringType.IsGenericType &&
			  invocation.Method.DeclaringType.GetGenericTypeDefinition() == typeof(IMocked<>))
			{
				// "Mixin" of IMocked<T>.Mock
				invocation.ReturnValue = this.Mock;
				return;
			}
			else if (invocation.Method.DeclaringType == typeof(IMocked))
			{
				// "Mixin" of IMocked.Mock
				invocation.ReturnValue = this.Mock;
				return;
			}

			// Special case for events.
			if (!FluentMockContext.IsActive)
			{
				if (invocation.Method.IsEventAttach())
				{
					var delegateInstance = (Delegate)invocation.Arguments[0];
					// TODO: validate we can get the event?
					var eventInfo = GetEventFromName(invocation.Method.Name.Substring(4));

					if (delegateInstance != null)
					{
						var mockEvent = delegateInstance.Target as MockedEvent;

						if (mockEvent != null)
						{
							mockEvent.Event = eventInfo;
						}
						else
						{
							this.Mock.AddEventHandler(eventInfo, (Delegate)invocation.Arguments[0]);
						}
					}

					return;
				}
				else if (invocation.Method.IsEventDetach())
				{
					var delegateInstance = (Delegate)invocation.Arguments[0];
					// TODO: validate we can get the event?
					var eventInfo = GetEventFromName(invocation.Method.Name.Substring(7));

					if (delegateInstance != null)
					{
						var mockEvent = delegateInstance.Target as MockedEvent;

						if (mockEvent != null)
						{
							mockEvent.Event = null;
						}
						else
						{
							this.Mock.RemoveEventHandler(eventInfo, (Delegate)invocation.Arguments[0]);
						}
					}

					return;
				}

				// Save to support Verify[expression] pattern.
				// In a fluent invocation context, which is a recorder-like 
				// mode we use to evaluate delegates by actually running them, 
				// we don't want to count the invocation, or actually run 
				// previous setups.
				actualInvocations.Add(invocation);
			}

			var call = FluentMockContext.IsActive ? (IProxyCall)null : orderedCalls.LastOrDefault(c => c.Matches(invocation));

			if (call == null && !FluentMockContext.IsActive && behavior == MockBehavior.Strict)
			{
				throw new MockException(MockException.ExceptionReason.NoSetup, behavior, invocation);
			}

			if (call != null)
			{
				call.SetOutParameters(invocation);

				// We first execute, as there may be a Throws 
				// and therefore we might never get to the 
				// next line.
				call.Execute(invocation);
				ThrowIfReturnValueRequired(call, invocation);
			}
			else if (invocation.Method.DeclaringType == typeof(object))
			{
				// Invoke underlying implementation.
				invocation.InvokeBase();
			}
			else if (invocation.TargetType.IsClass && !invocation.Method.IsAbstract && this.Mock.CallBase)
			{
				// For mocked classes, if the target method was not abstract, 
				// invoke directly.
				// Will only get here for Loose behavior.
				// TODO: we may want to provide a way to skip this by the user.
				invocation.InvokeBase();
			}
			else if (invocation.Method != null && invocation.Method.ReturnType != null &&
				invocation.Method.ReturnType != typeof(void))
			{
				Mock recursiveMock;
				if (this.Mock.InnerMocks.TryGetValue(invocation.Method, out recursiveMock))
				{
					invocation.ReturnValue = recursiveMock.Object;
				}
				else
				{
					invocation.ReturnValue = this.Mock.DefaultValueProvider.ProvideDefault(
						invocation.Method,
						invocation.Arguments);
				}
			}
		}

		/// <summary>
		/// Get an eventInfo for a given event name.  Search type ancestors depth first if necessary.
		/// </summary>
		/// <param name="eventName">Name of the event, with the set_ or get_ prefix already removed</param>
		private EventInfo GetEventFromName(string eventName)
		{
			var depthFirstProgress = new Queue<Type>();
			depthFirstProgress.Enqueue(targetType);
			while (depthFirstProgress.Count > 0)
			{
				var currentType = depthFirstProgress.Dequeue();
				var eventInfo = currentType.GetEvent(eventName);
				if (eventInfo != null)
				{
					return eventInfo;
				}

				foreach (var implementedType in GetAncestorTypes(currentType))
				{
					depthFirstProgress.Enqueue(implementedType);
				}
			}
			return null;
		}

		/// <summary>
		/// Given a type return all of its ancestors, both types and interfaces.
		/// </summary>
		/// <param name="initialType">The type to find immediate ancestors of</param>
		private static IEnumerable<Type> GetAncestorTypes(Type initialType)
		{
			Type baseType = initialType.BaseType;
			if (baseType != null)
			{
				return new[] { baseType };
			}

			return initialType.GetInterfaces();
		}

		private void ThrowIfReturnValueRequired(IProxyCall call, ICallContext invocation)
		{
			if (behavior != MockBehavior.Loose &&
				invocation.Method != null &&
				invocation.Method.ReturnType != null &&
				invocation.Method.ReturnType != typeof(void))
			{
				var methodCall = call as MethodCallReturn;
				if (methodCall == null || !methodCall.HasReturnValue)
				{
					throw new MockException(
						MockException.ExceptionReason.ReturnValueRequired,
						behavior,
						invocation);
				}
			}
		}

		private class ExpressionKey
		{
			private string fixedString;
			private List<object> values;

			public ExpressionKey(string fixedString, List<object> values)
			{
				this.fixedString = fixedString;
				this.values = values;
			}

			public override bool Equals(object obj)
			{
				if (object.ReferenceEquals(this, obj))
				{
					return true;
				}

				var key = obj as ExpressionKey;
				if (key == null)
				{
					return false;
				}

				var eq = key.fixedString == this.fixedString && key.values.Count == this.values.Count;

				var index = 0;
				while (eq && index < this.values.Count)
				{
					eq |= this.values[index] == key.values[index];
					index++;
				}

				return eq;
			}

			public override int GetHashCode()
			{
				var hash = fixedString.GetHashCode();

				foreach (var value in values)
				{
					if (value != null)
					{
						hash ^= value.GetHashCode();
					}
				}

				return hash;
			}
		}

		private class ConstantsVisitor : ExpressionVisitor
		{
			public ConstantsVisitor(Expression expression)
			{
				Values = new List<object>();
				base.Visit(expression);
			}

			public List<object> Values { get; set; }

			protected override Expression VisitConstant(ConstantExpression c)
			{
				Values.Add(c.Value);

				return base.VisitConstant(c);
			}
		}
	}
}