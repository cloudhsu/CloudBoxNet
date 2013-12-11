/*
* Copyright (c) 2011, Cloud Hsu
* All rights reserved.
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of the Cloud Hsu nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY CLOUD HSU "AS IS" AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace CloudBox.DesignPatterns
{
    /// <summary>
    /// Reflection Factory
    /// Using reflection to create a type of class and derive class
    /// </summary>
    /// <typeparam name="T">T is a base type</typeparam>
    public sealed class TReflectionFactory<T> : IFactory<T>
    {
        #region IFactory<T> Members

        /// <summary>
        /// Create and return a class with typeof(T)
        /// </summary>
        /// <returns>A base type class</returns>
        public T Create()
        {
            T t = default(T);
            try
            {
                t = (T)Activator.CreateInstance(typeof(T));
            }
            catch { }
            return t;
        }

        /// <summary>
        /// Create and return a class with typeof(T)
        /// </summary>
        /// <returns>A base type class</returns>
        public T Create(params object[] args)
        {
            T t = default(T);
#if !WindowsCE
            try
            {
                t = (T)Activator.CreateInstance(typeof(T), args);
            }
            catch { }
#else
            throw new NotSupportedException("Win CE not support CreateInstance(Type, object[])");
#endif
            return t;
        }

        /// <summary>
        /// Create and return a class
        /// </summary>
        /// <param name="type">type must a full name of class(EX:LPIMaintenanceData.PMChamber)</param>
        /// <returns>A base type class</returns>
        public T Create(string type)
        {
            T t = default(T);
            try
            {
#if !WindowsCE
                t = (T)Assembly.GetEntryAssembly().CreateInstance(type);
#else
                t = (T)Assembly.GetExecutingAssembly().CreateInstance(type);
#endif
                if (t == null)
                {
                    string dllname = type.Substring(0, type.LastIndexOf(".")) + ".dll";
                    t = (T)Assembly.LoadFrom(dllname).CreateInstance(type);
                }
            }
            catch { }
            return t;
        }

        /// <summary>
        /// Using Activator.CreateInstance to create object with parameters
        /// </summary>
        /// <param name="type">type must a full name of class(EX:LPIMaintenanceData.PMChamber)</param>
        /// <param name="args">parameters</param>
        /// <returns>A base type class</returns>
        public T Create(string type, params object[] args)
        {
            T t = default(T);
#if !WindowsCE
            try
            {
                t = (T)Assembly.GetEntryAssembly().CreateInstance(type, true,
                    BindingFlags.CreateInstance, null, args, null, null);
                if (t == null)
                {
                    string dllname = type.Substring(0, type.LastIndexOf(".")) + ".dll";
                    t = (T)Assembly.LoadFrom(dllname).CreateInstance(type, true,
                    BindingFlags.CreateInstance, null, args, null, null);
                }
            }
            catch { }
#else
            throw new NotSupportedException("Win CE not support CreateInstance(Type, object[])");
#endif
            return t;
        }

        #endregion
    }
}
