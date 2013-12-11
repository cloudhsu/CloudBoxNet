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
using System.Collections;

namespace CloudBox.DesignPatterns
{
    /// <summary>
    /// A key pair object pool.
    /// Using an unique key to manager pool.
    /// This pool is using to manage base classes and derive classes by unique keys.
    /// </summary>
    /// <typeparam name="T">T is a base type</typeparam>
    public sealed class TDictionaryPool<T> where T : IPoolable, new()
    {
        /// <summary>
        /// Using for pool
        /// </summary>
        static Dictionary<string, T> m_Pool;

        /// <summary>
        /// The static constructor only invoke once.
        /// So use to initial m_Pool collection.
        /// </summary>
        static TDictionaryPool()
        {
            m_Pool = new Dictionary<string, T>();
        }

        /// <summary>
        /// Not use, set private to disable new.
        /// </summary>
        private TDictionaryPool() { }

        /// <summary>
        /// Because the base classes and derive classes maybe created by Factory.
        /// So must using factory to create.
        /// </summary>
        /// <param name="a_Factory">A implement object for IFactory.</param>
        public static void RegisterFactory(IFactory<T> a_Factory)
        {
            Factory<T>.Register(a_Factory);
        }

        /// <summary>
        /// Use TDictionaryPool<X>.New(key) to get a instance.
        /// if the pool is empty, will new a object and invoke Create.
        /// if registered a factory, it will create by key.
        /// Every time invoke New function, it will invoke Initialize in IPoolable.
        /// Create object with typeof(T).FullName
        /// </summary>
        /// <param name="key">unique key in pool</param>
        /// <returns>An object in pool found by key or create by factory or new</returns>
        public static T New(string key)
        {
            T obj = default(T);
            if (m_Pool.ContainsKey(key))
                obj = m_Pool[key];
            if (obj == null)
            {
                obj = Factory<T>.Create();
                obj.Create();
            }
            obj.Initialize();
            return obj;
        }

        /// <summary>
        /// Using for create object with type
        /// </summary>
        /// <param name="key">unique key in pool</param>
        /// <param name="type">object type full name</param>
        /// <returns>An object in pool found by key or create by factory or new</returns>
        public static T New(string key, string type)
        {
            T obj = default(T);
            if (m_Pool.ContainsKey(key))
                obj = m_Pool[key];
            if (obj == null)
            {
                obj = Factory<T>.Create(type);
                if (obj == null)
                    throw new ArgumentNullException(type + " is not declare.");
                obj.Create();
            }
            obj.Initialize();
            return obj;
        }

        /// <summary>
        /// Create object with typeof(T).FullName
        /// </summary>
        /// <param name="key">unique key in pool</param>
        /// <param name="args">parameters for Create</param>
        /// <returns>An object in pool found by key or create by factory or new</returns>
        public static T New(string key, params object[] args)
        {
            T obj = default(T);
            if (m_Pool.ContainsKey(key))
                obj = m_Pool[key];
            if (obj == null)
            {
                obj = Factory<T>.Create();
                obj.Create(args);
            }
            obj.Initialize();
            return obj;
        }

        /// <summary>
        /// Create object with type string
        /// </summary>
        /// <param name="key">unique key in pool</param>
        /// <param name="type">type full name</param>
        /// <param name="args">parameters for Create</param>
        /// <returns>An object in pool found by key or create by factory or new</returns>
        public static T New(string key, string type, params object[] args)
        {
            T obj = default(T);
            if (m_Pool.ContainsKey(key))
                obj = m_Pool[key];
            if (obj == null)
            {
                obj = Factory<T>.Create(type);
                if (obj == null)
                    throw new ArgumentNullException(type + " is not declare.");
                obj.Create(args);
            }
            obj.Initialize();
            return obj;
        }

        /// <summary>
        /// When delete object, it will invoke the Release
        /// And you must give an unique key
        /// </summary>
        /// <param name="key">An unique key in pool</param>
        /// <param name="obj">Release object</param>
        public static void Delete(string key, T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Can't release null in pool");
            obj.Release();
            if (!m_Pool.ContainsKey(key))
                m_Pool.Add(key, obj);
        }

        /// <summary>
        /// Get the information for this pool.
        /// </summary>
        /// <returns>information</returns>
        public static new string ToString()
        {
            return typeof(T).ToString() + " Pool has [" + m_Pool.Count + "] items.";
        }
    }
}
