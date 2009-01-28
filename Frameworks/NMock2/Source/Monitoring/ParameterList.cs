//-----------------------------------------------------------------------
// <copyright file="ParameterList.cs" company="NMock2">
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
    using System.Collections;
    using System.Reflection;
    
    public class ParameterList
    {
        private MethodInfo method;
        private object[] values;
        private BitArray isValueSet;
        
        public ParameterList(MethodInfo method, object[] values)
        {
            this.method = method;
            this.values = values;
            this.isValueSet = new BitArray(values.Length);
            
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                this.isValueSet[i] = !parameters[i].IsOut;
            }
        }
        
        public int Count
        {
            get { return this.values.Length; }
        }

        public bool IsValueSet(int i)
        {
            return this.isValueSet[i];
        }

        public object this[int i]
        {
            get
            {
                if (this.IsValueSet(i))
                {
                    return this.values[i];
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Parameter '{0}' has not been set.", this.ParameterName(i)));
                }
            }

            set
            {
                if (this.CanValueBeSet(i))
                {
                    this.values[i] = value;
                    this.isValueSet[i] = true;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Cannot set the value of in parameter '{0}'", this.ParameterName(i)));
                }
            }
        }
        
        internal void MarkAllValuesAsSet()
        {
            this.isValueSet.SetAll(true);
        }

        internal object[] AsArray
        {
            get { return this.values; }
        }

        private bool CanValueBeSet(int i)
        {
            return !this.method.GetParameters()[i].IsIn;
        }
        
        private string ParameterName(int i)
        {
            return this.method.GetParameters()[i].Name;
        }
    }
}
