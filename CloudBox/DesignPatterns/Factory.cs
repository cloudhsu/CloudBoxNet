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
using System.Diagnostics;

namespace CloudBox.DesignPatterns
{
    /// <summary>
    /// A wrapper class to use IFactory.
    /// Default factory is TReflectionFactory
    /// </summary>
    /// <typeparam name="T">Product Type</typeparam>
    public sealed class Factory<T>
    {
        /// <summary>
        /// Factory instance
        /// </summary>
        static IFactory<T> m_DefaultFactory;
        static IFactory<T> m_CustomFactory;

        /// <summary>
        /// static constructor to initialize TReflectionFactory
        /// </summary>
        static Factory()
        {
            Debug.WriteLine("Create a [" + typeof(T).FullName + "] factory.");
            m_DefaultFactory = new TReflectionFactory<T>();
            m_CustomFactory = null;
        }

        /// <summary>
        /// register the factory
        /// it will created by the factory
        /// if factory is not exist, it will use TReflectionFactory to create
        /// </summary>
        /// <param name="factory">the type of IFactory</param>
        public static void Register(IFactory<T> factory)
        {
            m_CustomFactory = factory;
            Debug.WriteLine("Register a custom [" + m_CustomFactory.GetType().FullName + "] factory.");
        }

        /// <summary>
        /// Create product
        /// Using registered factory to create product
        /// If fail, create with default factory(TReflectionFactory)
        /// </summary>
        /// <returns>return null or a type instance</returns>
        public static T Create()
        {
            T t = default(T);
            if (m_CustomFactory != null)
            {
                t = m_CustomFactory.Create();
            }
            if (t == null)
            {
                t = m_DefaultFactory.Create();
            }
            return t;
        }

        /// <summary>
        /// Create product
        /// Using registered factory to create product
        /// If fail, create with default factory(TReflectionFactory)
        /// </summary>
        /// <returns>return null or a type instance</returns>
        public static T Create(params object[] args)
        {
            T t = default(T);
            if (m_CustomFactory != null)
            {
                t = m_CustomFactory.Create(args);
            }
            if (t == null)
            {
                t = m_DefaultFactory.Create(args);
            }
            return t;
        }

        /// <summary>
        /// Create product
        /// Using registered factory to create product
        /// If fail, create with default factory(TReflectionFactory)
        /// </summary>
        /// <param name="type">type name</param>
        /// <returns>return null or a type instance</returns>
        public static T Create(string type)
        {
            T t = default(T);
            if (m_CustomFactory != null)
            {
                t = m_CustomFactory.Create(type);
            }
            if (t == null)
            {
                t = m_DefaultFactory.Create(type);
            }
            return t;
        }

        /// <summary>
        /// Create product with parameters
        /// Using registered factory to create product
        /// If fail, create with default factory(TReflectionFactory)
        /// </summary>
        /// <param name="type">type name</param>
        /// <param name="args">parameters</param>
        /// <returns>return null or a type instance</returns>
        public static T Create(string type, params object[] args)
        {
            T t = default(T);
            if (m_CustomFactory != null)
            {
                t = m_CustomFactory.Create(type, args);
            }
            if (t == null)
            {
                t = m_DefaultFactory.Create(type, args);
            }
            return t;
        }
    }
}
