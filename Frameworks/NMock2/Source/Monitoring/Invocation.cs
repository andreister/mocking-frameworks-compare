//-----------------------------------------------------------------------
// <copyright file="Invocation.cs" company="NMock2">
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
namespace NMock2.Monitoring
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Represents the invocation of a method on an object (receiver).
    /// </summary>
    public class Invocation : ISelfDescribing
    {
        public readonly object Receiver;
        public readonly MethodInfo Method;
        public readonly ParameterList Parameters;
        
        private object result = Missing.Value;
        private Exception exception = null;
        private bool isThrowing = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Invocation"/> class.
        /// </summary>
        /// <param name="receiver">The receiver providing the method.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters passed to the method..</param>
        public Invocation(object receiver, MethodInfo method, object[] parameters)
        {
            this.Receiver = receiver;
            this.Method = method;
            this.Parameters = new ParameterList(method, parameters);
        }

        /// <summary>
        /// Gets or sets the result of the invocation.
        /// </summary>
        /// <value>The result.</value>
        public object Result
        {
            get
            {
                return this.result;
            }

            set
            {
                this.CheckReturnType(value);

                this.result = value;
                this.exception = null;
                this.isThrowing = false;
            }
        }

        /// <summary>
        /// Gets or sets the exception that is thrown on the invocation.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get
            {
                return this.exception;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.exception = value;
                this.result = null;
                this.isThrowing = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an exception is thrown an this invocation.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this invocation is throwing an exception; otherwise, <c>false</c>.
        /// </value>
        public bool IsThrowing
        {
            get { return this.isThrowing; }
        }

        /// <summary>
        /// Invokes this invocation on the specified receiver and stores the result and exception
        /// returns/thrown by the invocation.
        /// </summary>
        /// <param name="otherReceiver">The other receiver.</param>
        public void InvokeOn(object otherReceiver)
        {
            try
            {
                this.Result = this.Method.Invoke(otherReceiver, Parameters.AsArray);
                this.Parameters.MarkAllValuesAsSet();
            }
            catch (TargetInvocationException e)
            {
                Exception = e.InnerException;
            }
        }

        /// <summary>
        /// Describes this object.
        /// </summary>
        /// <param name="writer">The text writer the description is added to.</param>
        public void DescribeTo(TextWriter writer)
        {
            writer.Write(this.Receiver.ToString());

            if (this.MethodIsIndexerGetter())
            {
                this.DescribeAsIndexerGetter(writer);
            }
            else if (this.MethodIsIndexerSetter())
            {
                this.DescribeAsIndexerSetter(writer);
            }
            else if (this.MethodIsEventAdder())
            {
                this.DescribeAsEventAdder(writer);
            }
            else if (this.MethodIsEventRemover())
            {
                this.DescribeAsEventRemover(writer);
            }
            else if (this.MethodIsProperty())
            {
                this.DescribeAsProperty(writer);
            }
            else
            {
                this.DescribeNormalMethod(writer);
            }
        }

        private void CheckReturnType(object value)
        {
            if (this.Method.ReturnType == typeof(void) && value != null)
            {
                throw new ArgumentException("cannot return a value from a void method", "value");
            }

            if (this.Method.ReturnType != typeof(void) && this.Method.ReturnType.IsValueType && value == null)
            {
                if (!(this.Method.ReturnType.IsGenericType && this.Method.ReturnType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    throw new ArgumentException("cannot return a null value type", "value");
                }
            }

            if (value != null && !this.Method.ReturnType.IsInstanceOfType(value))
            {
                throw new ArgumentException(
                    "cannot return a value of type " + this.DescribeType(value) + " from a method returning " + this.Method.ReturnType,
                    "value");
            }
        }

        private bool MethodIsProperty()
        {
            return this.Method.IsSpecialName &&
                   ((this.Method.Name.StartsWith("get_") && this.Parameters.Count == 0) ||
                    (this.Method.Name.StartsWith("set_") && this.Parameters.Count == 1));
        }
        
        private bool MethodIsIndexerGetter()
        {
            return this.Method.IsSpecialName
                && this.Method.Name == "get_Item"
                && this.Parameters.Count >= 1;
        }

        private bool MethodIsIndexerSetter()
        {
            return this.Method.IsSpecialName
                && this.Method.Name == "set_Item"
                && this.Parameters.Count >= 2;
        }
        
        private bool MethodIsEventAdder()
        {
            return this.Method.IsSpecialName
                && this.Method.Name.StartsWith("add_")
                && this.Parameters.Count == 1
                && typeof(Delegate).IsAssignableFrom(this.Method.GetParameters()[0].ParameterType);
        }
        
        private bool MethodIsEventRemover()
        {
            return this.Method.IsSpecialName
                && this.Method.Name.StartsWith("remove_")
                && this.Parameters.Count == 1
                && typeof(Delegate).IsAssignableFrom(this.Method.GetParameters()[0].ParameterType);
        }

        private void DescribeAsProperty(TextWriter writer)
        {
            writer.Write(".");
            writer.Write(this.Method.Name.Substring(4));
            if (this.Parameters.Count > 0)
            {
                writer.Write(" = ");
                writer.Write(this.Parameters[0]);
            }
        }

        private void DescribeAsIndexerGetter(TextWriter writer)
        {
            writer.Write("[");
            this.WriteParameterList(writer, this.Parameters.Count);
            writer.Write("]");
        }
        
        private void DescribeAsIndexerSetter(TextWriter writer)
        {
            writer.Write("[");
            this.WriteParameterList(writer, this.Parameters.Count - 1);
            writer.Write("] = ");
            writer.Write(this.Parameters[this.Parameters.Count - 1]);
        }
        
        private void DescribeNormalMethod(TextWriter writer)
        {
            writer.Write(".");
            writer.Write(this.Method.Name);

            this.WriteTypeParams(writer);

            writer.Write("(");
            this.WriteParameterList(writer, this.Parameters.Count);
            writer.Write(")");
        }

        private void WriteTypeParams(TextWriter writer)
        {
            Type[] types = this.Method.GetGenericArguments();
            if (types.Length > 0)
            {
                writer.Write("<");

                for (int i = 0; i < types.Length; i++)
                {
                    if (i > 0)
                    {
                        writer.Write(", ");
                    }

                    writer.Write(types[i].FullName);
                }

                writer.Write(">");
            }
        }

        private void WriteParameterList(TextWriter writer, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                if (this.Method.GetParameters()[i].IsOut)
                {
                    writer.Write("out");
                }
                else
                {
                    writer.Write(this.Parameters[i]);
                }
            }
        }
        
        private void DescribeAsEventAdder(TextWriter writer)
        {
            writer.Write(".");
            writer.Write(this.Method.Name.Substring(4));
            writer.Write(" += ");
            writer.Write(this.Parameters[0]);
        }

        private void DescribeAsEventRemover(TextWriter writer)
        {
            writer.Write(".");
            writer.Write(this.Method.Name.Substring(7));
            writer.Write(" -= ");
            writer.Write(this.Parameters[0]);
        }

        private string DescribeType(object o)
        {
            Type type = o.GetType();
            StringBuilder sb = new StringBuilder();
            sb.Append(type);

            Type[] interfaceTypes = type.GetInterfaces();
            if (interfaceTypes.Length > 0)
            {
                sb.Append(": ");

                foreach (Type interfaceType in interfaceTypes)
                {
                    sb.Append(interfaceType);
                    sb.Append(", ");
                }

                sb.Length -= 2; // cut away last ", "
            }

            return sb.ToString();
        }
    }
}
