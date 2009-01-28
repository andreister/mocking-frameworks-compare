using System;
using NMock2;
using NMock2.Syntax;

namespace NMock2Tests
{
    /// <summary>
    /// Extension methods to make NMock2 type safe. 
    /// While seemingly powerful, it's quite dumb and does not support advanced scenarios like indexers etc.
    /// </summary>
    internal static class NMock2Extensions
    {
        internal static T NewMock<T>(this Mockery mockery)
        {
            return (T) mockery.NewMock(typeof (T));    
        }

        internal static IMethodSyntax On<Caller>(this IReceiverSyntax syntax, Caller x)
        {
            return syntax.On(x);
        }

        internal static IArgumentSyntax Method<Parameter>(this IMethodSyntax syntax, Action<Parameter> action)
        {
            string name = action.Method.Name;
            return syntax.Method(name);
        }

        internal static IArgumentSyntax Method(this IMethodSyntax syntax, Action action)
        {
            string name = action.Method.Name;
            return syntax.Method(name);
        }
    }
}
