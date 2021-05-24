using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Singletons;

namespace Utilities.Databases
{
    public class Database<K,T> : SerializedScriptableObject
    {
        public T this[K key] => _datas[key];
        
        [SerializeField]
        private Dictionary<K, T> _datas = new Dictionary<K, T>();
    }
    
    public class Database<T,K,V> : SerializedSingletonScriptableObject<T> where T : SerializedScriptableObject
    {
        public V this[K key] => _datas[key];

        [SerializeField]
        private Dictionary<K, V> _datas = new Dictionary<K, V>();
    }
}