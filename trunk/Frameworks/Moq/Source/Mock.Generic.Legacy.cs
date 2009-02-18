﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Moq.Language.Flow;
using System.Linq.Expressions;

namespace Moq
{
	// Keeps legacy members that are hidden and are provided 
	// for backwards compatibility (so that existing projects 
	// still compile, but people don't see them).
	// A bug in EditorBrowsable actually prevents us from moving these members 
	// completely to extension methods, as the attribute is not honored and 
	// therefore the members are always visible.

	public partial class Mock<T>
	{
		/// <summary>
		/// Sets an expectation on the mocked type for a call to 
		/// to a void method.
		/// </summary>
		/// <remarks>
		/// If more than one expectation is set for the same method or property, 
		/// the latest one wins and is the one that will be executed.
		/// </remarks>
		/// <param name="expression">Lambda expression that specifies the expected method invocation.</param>
		/// <example group="expectations">
		/// <code>
		/// var mock = new Mock&lt;IProcessor&gt;();
		/// this.Setup(x =&gt; x.Execute("ping"));
		/// </code>
		/// </example>
		[Obsolete("Expect has been renamed to Setup.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ISetup<T> Expect(Expression<Action<T>> expression)
		{
			return Setup(expression);
		}

		/// <summary>
		/// Sets an expectation on the mocked type for a call to 
		/// to a value returning method.
		/// </summary>
		/// <typeparam name="TResult">Type of the return value. Typically omitted as it can be inferred from the expression.</typeparam>
		/// <remarks>
		/// If more than one expectation is set for the same method or property, 
		/// the latest one wins and is the one that will be executed.
		/// </remarks>
		/// <param name="expression">Lambda expression that specifies the expected method invocation.</param>
		/// <example group="expectations">
		/// <code>
		/// this.Setup(x =&gt; x.HasInventory("Talisker", 50)).Returns(true);
		/// </code>
		/// </example>
		[Obsolete("Expect has been renamed to Setup.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ISetup<T, TResult> Expect<TResult>(Expression<Func<T, TResult>> expression)
		{
			return Setup(expression);
		}

		/// <summary>
		/// Sets an expectation on the mocked type for a call to 
		/// to a property getter.
		/// </summary>
		/// <remarks>
		/// If more than one expectation is set for the same property getter, 
		/// the latest one wins and is the one that will be executed.
		/// </remarks>
		/// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
		/// <param name="expression">Lambda expression that specifies the expected property getter.</param>
		/// <example group="expectations">
		/// <code>
		/// this.SetupGet(x =&gt; x.Suspended)
		///     .Returns(true);
		/// </code>
		/// </example>
		[Obsolete("ExpectGet has been renamed to SetupGet.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ISetupGetter<T, TProperty> ExpectGet<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			return SetupGet(expression);
		}

		/// <summary>
		/// Sets an expectation on the mocked type for a call to 
		/// to a property setter.
		/// </summary>
		/// <remarks>
		/// If more than one expectation is set for the same property setter, 
		/// the latest one wins and is the one that will be executed.
		/// </remarks>
		/// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
		/// <param name="expression">Lambda expression that specifies the expected property setter.</param>
		/// <example group="expectations">
		/// <code>
		/// this.SetupSet(x =&gt; x.Suspended);
		/// </code>
		/// </example>
		[Obsolete("ExpectSet has been renamed to SetupSet.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ISetupSetter<T, TProperty> ExpectSet<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			return Mock.SetupSet(this, expression);
		}

		/// <summary>
		/// Sets an expectation on the mocked type for a call to 
		/// to a property setter with a specific value.
		/// </summary>
		/// <remarks>
		/// More than one expectation can be set for the setter with 
		/// different values.
		/// </remarks>
		/// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
		/// <param name="expression">Lambda expression that specifies the expected property setter.</param>
		/// <param name="value">The value expected to be set for the property.</param>
		/// <example group="expectations">
		/// <code>
		/// this.SetupSet(x =&gt; x.Suspended, true);
		/// </code>
		/// </example>
		[Obsolete("ExpectSet has been renamed to SetupSet.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ISetupSetter<T, TProperty> ExpectSet<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
		{
			return Mock.SetupSet(this, expression, value);
		}
	}

	/// <summary>
	/// Holds extensions that would cause conflicts with new APIs if available 
	/// in the core Moq namespace (even if hidden), such as the SetupSet legacy 
	/// members.
	/// </summary>
	public static class MockLegacyExtensions
	{
		/// <summary>
		/// Specifies a setup on the mocked type for a call to 
		/// to a property setter.
		/// </summary>
		/// <remarks>
		/// If more than one setup is set for the same property setter, 
		/// the latest one wins and is the one that will be executed.
		/// </remarks>
		/// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
		/// <typeparam name="T">Type of the mock.</typeparam>
		/// <param name="mock">The target mock for the setup.</param>
		/// <param name="expression">Lambda expression that specifies the property setter.</param>
		/// <example group="setups">
		/// <code>
		/// mock.SetupSet(x =&gt; x.Suspended);
		/// </code>
		/// </example>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static ISetupSetter<T, TProperty> SetupSet<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> expression)
			where T : class
		{
			return Mock.SetupSet<T, TProperty>(mock, expression);
		}

		/// <summary>
		/// Specifies a setup on the mocked type for a call to 
		/// to a property setter with a specific value.
		/// </summary>
		/// <remarks>
		/// More than one setup can be set for the setter with 
		/// different values.
		/// </remarks>
		/// <typeparam name="TProperty">Type of the property. Typically omitted as it can be inferred from the expression.</typeparam>
		/// <typeparam name="T">Type of the mock.</typeparam>
		/// <param name="mock">The target mock for the setup.</param>
		/// <param name="expression">Lambda expression that specifies the property setter.</param>
		/// <param name="value">The value to be set for the property.</param>
		/// <example group="setups">
		/// <code>
		/// mock.SetupSet(x =&gt; x.Suspended, true);
		/// </code>
		/// </example>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static ISetupSetter<T, TProperty> SetupSet<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> expression, TProperty value)
			where T : class
		{
			return Mock.SetupSet(mock, expression, value);
		}

		/// <summary>
		/// Verifies that a property has been set on the mock. 
		/// Use in conjuntion with the default <see cref="MockBehavior.Loose"/>.
		/// </summary>
		/// <example group="verification">
		/// This example assumes that the mock has been used, 
		/// and later we want to verify that a given invocation 
		/// with specific parameters was performed:
		/// <code>
		/// var mock = new Mock&lt;IWarehouse&gt;();
		/// // exercise mock
		/// //...
		/// // Will throw if the test code didn't set the IsClosed property.
		/// mock.VerifySet(warehouse =&gt; warehouse.IsClosed);
		/// </code>
		/// </example>
		/// <exception cref="MockException">The invocation was not performed on the mock.</exception>
		/// <param name="expression">Expression to verify.</param>
		/// <param name="mock">The mock instance.</param>
		/// <typeparam name="T">Mocked type.</typeparam>
		/// <typeparam name="TProperty">Type of the property to verify. Typically omitted as it can 
		/// be inferred from the expression's return type.</typeparam>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void VerifySet<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> expression)
			where T : class
		{
			Mock.VerifySet(mock, expression, Times.AtLeastOnce(), null);
		}

		/// <summary>
		/// Verifies that a property has been set on the mock to the given value.
		/// Use in conjuntion with the default <see cref="MockBehavior.Loose"/>.
		/// </summary>
		/// <example group="verification">
		/// This example assumes that the mock has been used, 
		/// and later we want to verify that a given invocation 
		/// with specific parameters was performed:
		/// <code>
		/// var mock = new Mock&lt;IWarehouse&gt;();
		/// // exercise mock
		/// //...
		/// // Will throw if the test code didn't set the IsClosed property to true
		/// mock.VerifySet(warehouse =&gt; warehouse.IsClosed, true);
		/// </code>
		/// </example>
		/// <exception cref="MockException">The invocation was not performed on the mock.</exception>
		/// <param name="expression">Expression to verify.</param>
		/// <param name="value">The value that should have been set on the property.</param>
		/// <param name="mock">The mock instance.</param>
		/// <typeparam name="T">Mocked type.</typeparam>
		/// <typeparam name="TProperty">Type of the property to verify. Typically omitted as it can 
		/// be inferred from the expression's return type.</typeparam>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void VerifySet<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> expression, TProperty value)
			where T : class
		{
			Mock.VerifySet(mock, expression, value, Times.AtLeastOnce(), null);
		}

		/// <summary>
		/// Verifies that a property has been set on the mock, specifying a failure  
		/// error message. 
		/// Use in conjuntion with the default <see cref="MockBehavior.Loose"/>.
		/// </summary>
		/// <example group="verification">
		/// This example assumes that the mock has been used, 
		/// and later we want to verify that a given invocation 
		/// with specific parameters was performed:
		/// <code>
		/// var mock = new Mock&lt;IWarehouse&gt;();
		/// // exercise mock
		/// //...
		/// // Will throw if the test code didn't set the IsClosed property.
		/// mock.VerifySet(warehouse =&gt; warehouse.IsClosed);
		/// </code>
		/// </example>
		/// <exception cref="MockException">The invocation was not performed on the mock.</exception>
		/// <param name="expression">Expression to verify.</param>
		/// <param name="failMessage">Message to show if verification fails.</param>
		/// <param name="mock">The mock instance.</param>
		/// <typeparam name="T">Mocked type.</typeparam>
		/// <typeparam name="TProperty">Type of the property to verify. Typically omitted as it can 
		/// be inferred from the expression's return type.</typeparam>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void VerifySet<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> expression, string failMessage)
			where T : class
		{
			Mock.VerifySet(mock, expression, Times.AtLeastOnce(), failMessage);
		}

		/// <summary>
		/// Verifies that a property has been set on the mock to the given value, specifying a failure  
		/// error message. 
		/// Use in conjuntion with the default <see cref="MockBehavior.Loose"/>.
		/// </summary>
		/// <example group="verification">
		/// This example assumes that the mock has been used, 
		/// and later we want to verify that a given invocation 
		/// with specific parameters was performed:
		/// <code>
		/// var mock = new Mock&lt;IWarehouse&gt;();
		/// // exercise mock
		/// //...
		/// // Will throw if the test code didn't set the IsClosed property to true
		/// mock.VerifySet(warehouse =&gt; warehouse.IsClosed, true);
		/// </code>
		/// </example>
		/// <exception cref="MockException">The invocation was not performed on the mock.</exception>
		/// <param name="expression">Expression to verify.</param>
		/// <param name="value">The value that should have been set on the property.</param>
		/// <param name="failMessage">Message to show if verification fails.</param>
		/// <param name="mock">The mock instance.</param>
		/// <typeparam name="T">Mocked type.</typeparam>
		/// <typeparam name="TProperty">Type of the property to verify. Typically omitted as it can 
		/// be inferred from the expression's return type.</typeparam>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void VerifySet<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> expression, TProperty value, string failMessage)
			where T : class
		{
			Mock.VerifySet(mock, expression, value, Times.AtLeastOnce(), failMessage);
		}
	}
}