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
using System.Linq;
using System.Text;
using System.Collections;

namespace CloudBox.DesignPatterns
{
    public class IndexCache :_IndexCache,IIndexCache
    {
        /// <summary>
        /// Using for pool
        /// </summary>
        ArrayList m_Pool;

         /// <summary>
        /// Not use, set private to disable new.
        /// </summary>
        public IndexCache() : base()
        {
            m_Pool = new ArrayList();
        }

        #region IIndexCache Members

        /// <summary>
        /// Use IndexCache<X>.New(index) to get a instance.
        /// if the pool is empty or object is not using will return null.
        /// </summary>
        /// <param name="index">index in cache</param>
        /// <returns>T is a type of class</returns>
        public object New(int index)
        {
            object obj = null;
            if (index < m_Pool.Count && m_objState[index] == ObjectState.NO_USE)
            {
                obj = m_Pool[index];
                m_objState[index] = ObjectState.USING;
            }
            else if (m_objState[index] == ObjectState.USING)
            {
                throw new Exception("[" + index + "] was be used.");
            }
            else if (index >= m_Pool.Count)
            {
                throw new Exception("[" + index + "] not exist in cache.");
            }
            return obj;
        }

        /// <summary>
        /// Put back into cache
        /// </summary>
        /// <param name="index">object index in cache</param>
        public void Delete(int index)
        {
            if (index < m_Pool.Count)
            {
                m_objState[index] = ObjectState.NO_USE;
            }
            else
            {
                throw new Exception("[" + index + "] not exist in cache.");
            }
        }

        /// <summary>
        /// Register an object in cache
        /// </summary>
        /// <param name="obj">An object to register</param>
        public void Register(object obj)
        {
            m_Pool.Add(obj);
            m_objState.Add(ObjectState.NO_USE);
        }

        /// <summary>
        /// Unregister an object.
        /// </summary>
        /// <param name="obj">An object to unregister</param>
        public void Unregister(object obj)
        {
            int index = m_Pool.IndexOf(obj);
            m_Pool.Remove(obj);
            m_objState.RemoveAt(index);
        }

        #endregion

        /// <summary>
        /// Get the information for this cache.
        /// </summary>
        /// <returns>information</returns>
        public new string ToString()
        {
            return typeof(object).ToString() + " IndexCache has [" + m_Pool.Count + "] items.";
        }
    }
}
