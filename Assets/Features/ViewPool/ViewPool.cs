using System.Collections.Generic;
using UnityEngine;

namespace Features.ViewPool
{
    public class ViewPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly Stack<T> _pool = new();

        private readonly T _prefab;

        public ViewPool(T prefab)
        {
            _prefab = prefab;
        }

        public T GetOrCreate()
        {
            var item = _pool.Count > 0 ? _pool.Pop() : CreateNew();
            item.gameObject.SetActive(true);

            return item;
        }

        public void AddToPool(T item)
        {
            _pool.Push(item);
        }

        private T CreateNew()
        {
            var newInstance = Object.Instantiate(_prefab);
            newInstance.SetupPool(this);
            return newInstance;
        }
    }

    public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
    {
        void SetupPool(ViewPool<T> pool);
    }
}