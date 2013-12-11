using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.DesignPatterns
{
    public interface IIndexCache
    {
        void Register(object obj);
        void Unregister(object obj);
        object New(int index);
        void Delete(int index);
    }

    public interface IIndexCache<T>
    {
        void Register(T obj);
        void Unregister(T obj);
        T New(int index);
        void Delete(int index);
    }
}
