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

namespace CloudBox.DesignPatterns
{
    /// <summary>
    /// This is a generic object pool.
    /// </summary>
    /// <typeparam name="T">T is a class which implement IPoolable</typeparam>
    public class TObjectPool<T> where T : IPoolable, new()
    {
        /// <summary>
        /// Maximum for pool.
        /// </summary>
        static int m_PoolMaxNum;

        /// <summary>
        /// Using for pool
        /// </summary>
        static Stack<T> m_Pool;

        /// <summary>
        /// The static constructor only invoke once.
        /// So use to initial m_Pool collection.
        /// </summary>
        static TObjectPool()
        {
            m_PoolMaxNum = 10;
            m_Pool = new Stack<T>(m_PoolMaxNum);
        }

        /// <summary>
        /// Not use, set private to disable new.
        /// </summary>
        private TObjectPool(){}

        /// <summary>
        /// Maximum for pool.
        /// </summary>
        public static int PoolMaxNum
        {
            get { return m_PoolMaxNum; }
            set { m_PoolMaxNum = value; }
        }

        /// <summary>
        /// Use ObjectPool<X>.New() to get a instance.
        /// if the pool is empty, will new a object and invoke Create.
        /// Every time invoke New function, it will invoke Initialize in IPoolable.
        /// </summary>
        /// <returns>T is a type of class</returns>
        public static T New()
        {
            T obj = default(T);
            if (m_Pool.Count > 0)
                obj = m_Pool.Pop();
            if (obj == null)
            {
                obj = Factory<T>.Create();
                obj.Create();
            }
            obj.Initialize();
            return obj;
        }

        public static T New(params object[] args)
        {
            T obj = default(T);
            if (m_Pool.Count > 0)
                obj = m_Pool.Pop();
            if (obj == null)
            {
                obj = Factory<T>.Create(args);
                obj.Create(args);
            }
            obj.Initialize();
            return obj;
        }

        /// <summary>
        /// When delete object, it will invoke the Release
        /// If pool was full, it will collect by GC.
        /// </summary>
        /// <param name="obj">release object.</param>
        public static void Delete(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Can't release null in pool");
            obj.Release();
            if (m_Pool.Count <= m_PoolMaxNum)
                m_Pool.Push(obj);
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
