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
    /// A generic factory, implement to create product.
    /// </summary>
    /// <typeparam name="T">Product Type</typeparam>
    public interface IFactory<T>
    {
        /// <summary>
        /// Create and return an object
        /// </summary>
        /// <returns>T is a type of object</returns>
        T Create();

        /// <summary>
        /// Create and return an object
        /// </summary>
        /// <param name="args">parameters</param>
        /// <returns>T is a type of object</returns>
        T Create(params object[] args);

        /// <summary>
        /// Create and return an object
        /// </summary>
        /// <param name="type">product type full name</param>
        /// <returns>T is a type of object</returns>
        T Create(string type);

        /// <summary>
        /// Create with parameter constructor
        /// </summary>
        /// <param name="type">product type full name</param>
        /// <param name="args">parameters</param>
        /// <returns>T is a type of object</returns>
        T Create(string type, params object[] args);
    }
}
