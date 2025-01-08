using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
    public class ObjectPool
    {
        private GameObject _prefab;
        private GameObject _parent;
        private bool _allowExpand;
        
        private List<GameObject> _pool = new();
        
        
        public ObjectPool(GameObject prefab, int amount, bool allowExpand = false)
        {
            _prefab = prefab;
            _allowExpand = allowExpand;

            _parent = new GameObject() { name = prefab.name + "Pool" };

            for (int i = 0; i < amount; i++)
            {
                GameObject obj = MonoBehaviour.Instantiate(_prefab, _parent.transform, true);
                obj.SetActive(false);
                _pool.Add(obj);   
            }
        }
        
        public GameObject GetObject()
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (!_pool[i].GameObject().activeInHierarchy)
                {
                    return _pool[i];
                }
            }

            if (_allowExpand)
            {
                return AddObjectToPool();
            }
            
            return null;
        }
        
        
        public GameObject ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            
            if (obj.transform.parent != _parent.transform)
            {
                obj.transform.SetParent(_parent.transform);
            }
            
            obj.transform.parent = _parent.transform;
            
            return obj;
        }


    private GameObject AddObjectToPool()
    {
        GameObject obj = MonoBehaviour.Instantiate(_prefab, _parent.transform, true);
        obj.SetActive(false);
        _pool.Add(obj);

        return obj;
    }
    }
}