using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    interface ICost
    {
        double getCost();
    }

    class EmptyCost : ICost
    {

        #region ICost Members

        public double getCost()
        {
            return 0;
        }

        #endregion
    }

    abstract class Decorator
    {
        protected ICost m_cost;

        public Decorator()
        {
            m_cost = new EmptyCost();
        }

        public Decorator(ICost cost)
        {
            m_cost = cost;
        }
    }

    abstract class Decorator<T>
    {
        protected T m_cost;

        public Decorator()
        {
            //m_cost = new EmptyCost();
        }

        public Decorator(T cost)
        {
            m_cost = cost;
        }
    }

    class Decorator<T1, T2> : Decorator<T2>,ICost
        where T1 : ICost, new()
        where T2 : ICost, new()
    {

        #region ICost Members

        public double getCost()
        {
            T2 t = new T2();
            return m_cost.getCost() + t.getCost();
        }

        #endregion
    }

    class Apple : Decorator, ICost
    {
        public Apple() : base() { }
        public Apple(ICost cost) : base(cost){}

        public double getCost()
        {
            return m_cost.getCost() + 11;
        }
    }

    class Banana : Decorator, ICost
    {
        public Banana() : base() { }
        public Banana(ICost cost) : base(cost) { }
        public double getCost()
        {
            return m_cost.getCost() + 12;
        }
    }
}
