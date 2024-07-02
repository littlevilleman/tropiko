using System.Collections.Generic;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public interface IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        public abstract T Pull(Transform parent);
        public abstract void Recycle(T element);
    }
    public interface IPoolable<T>
    {
        public event RecyclePoolable<T> onRecycle;
    }

    public abstract class Pool<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        //[SerializeField] private Transform world;
        [SerializeField] private T prefab;
        [SerializeField] private int instances = 10;
        [SerializeField] private bool instanceOnDemand = true;

        protected List<T> elements;


        private void Awake()
        {
            elements = new List<T>();
            for (int i = 0; i < instances; i++)
            {
                Recycle(CreateElement());
            }
        }

        public T Pull(Transform parent)
        {
            foreach (T element in elements)
            {
                return Pull(element, parent);
            }

            if (instanceOnDemand)
                return Pull(CreateElement(), parent);

            Debug.LogError($"No instances found for {typeof(T)}");
            return null;
        }

        private T CreateElement()
        {
            T element = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            return element;
        }

        private T Pull(T element, Transform parent)
        {
            element.transform.SetParent(parent);
            elements.Remove(element);
            return element;
        }

        public void Recycle(T element)
        {
            element.onRecycle -= Recycle;
            element.transform.SetParent(transform);
            element.transform.rotation = Quaternion.identity;
            element.transform.localPosition = Vector3.zero;
            element.gameObject.SetActive(false);
            elements.Add(element);
        }
    }
}