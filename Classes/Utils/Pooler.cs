#if RELEASE_BUILD
using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace SuperFramework.Core.Utils
{
    public abstract class Pooler<T> where T : class, IPoolable
    {

        public int Count => _cachedItems.Count;

        protected Stack<T> _cachedItems;
        protected Func<T> _factoryFunction;
        protected readonly ILogger _logger;

        public Pooler(ILogger logger, Func<T> factoryFunction, int capacity = 20)
        {

            if (factoryFunction == null)
            {
                throw new ArgumentNullException("Factory function can not be null!");
            }

            _factoryFunction = factoryFunction;
            _logger = logger;
            _cachedItems = new Stack<T>(capacity);
            Populate(capacity);

        }

        public virtual void Populate(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                _cachedItems.Push(_factoryFunction());
            }
        }


        public virtual T Obtain()
        {
            T item;

            if (_cachedItems.Count == 0)
            {
                item =  _factoryFunction();
            }
            else
            {
                item = _cachedItems.Pop();
            }
            
            item.OnGet();

            return item;
        }

        public virtual void Release(T item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("Item can not be null!");
            }
            item.OnReturn();
            _cachedItems.Push(item);
        }

        public virtual void RemoveAllItems(Action<T> removeAction)
        {
            if (_cachedItems.Count == 0)
                return;

            T item = _cachedItems.Pop();
            try
            {
                removeAction(item);

                RemoveAllItems(removeAction);

            }
            catch(Exception e)
            {
                _logger.LogException("Remove all items from pooler failed!",e);
            }
         
        }
    }
}
#endif