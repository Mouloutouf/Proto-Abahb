using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Utilities.Runtime.NeUI.Sync
{
    [CreateAssetMenu(fileName = "New Color Palette", menuName = "UI Data/Color Palette", order = 0)]
    public class SyncDatabase<T> : SerializedScriptableObject
    {
        [SerializeField]
        private List<SyncData<T>> datas = new List<SyncData<T>>();

        private Dictionary<string, SyncData<T>> dataDictionary;
        public string Default()
        {
            return datas.First().Name;
        }
        public List<string> Keys()
        {
            return datas.Select(p => p.Name).ToList();
        }

        protected override void OnAfterDeserialize()
        {
            datas.ForEach(d => d.ClearSubscribers());
        }

        [Button]
        public void UpdateCurrentScene()
        {
            datas.ForEach(d => d.ClearSubscribers());
            FindObjectsOfType<SyncedComponent<T>>().Where(o => o.database == this).ForEach(c => c.RefreshValue());
            #if UNITY_EDITOR
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
            #endif
        }

        private void FillDictionary()
        {
            dataDictionary = new Dictionary<string, SyncData<T>>();
            for (int i = 0; i < datas.Count; i++)
            {
                dataDictionary.Add(datas[i].Name, datas[i]);
            }
        }

        public T FetchValue(string key)
        {
            if (dataDictionary == null) FillDictionary();
            return dataDictionary[key].Value;
        }
        public void SubscribeTo(SyncedComponent<T> target, string key)
        {
            var color = datas.Find(c => c.Name == key);
            if(color == null) return;
            color.Subscribe(target);
        }
        
        public void UnsubscribeFrom(SyncedComponent<T> target, string key)
        {
            var color = datas.Find(c => c.Name == key);
            if(color == null) return;
            color.Unsubscribe(target);
        }
    }

    [Serializable, InlineProperty, HideReferenceObjectPicker]
    public class SyncData<T>
    {
        [OnValueChanged("UpdateName")] 
        public string Name;
        [OnValueChanged("Notify")]
        public T Value;

        private List<SyncedComponent<T>> Subscribers = new List<SyncedComponent<T>>();

        public void Subscribe(SyncedComponent<T> subscriber)
        {
            if (!Subscribers.Contains(subscriber))
            {
                Subscribers.Add(subscriber);
                subscriber.UpdateValue(Value);
            }
        }

        public void Unsubscribe(SyncedComponent<T> subscriber)
        {
            if (Subscribers.Contains(subscriber)) Subscribers.Remove(subscriber);
        }

        public void Notify()
        {
            List<int> removeList = new List<int>();
            for (int i = 0; i < Subscribers.Count; i++)
            {
                var subscriber = Subscribers[i];
                if(subscriber == null) removeList.Add(i);
                else subscriber.UpdateValue(Value);
            }

            removeList.Reverse();
            for (int i = 0; i < removeList.Count; i++)
            {
                Subscribers.RemoveAt(removeList[i]);
            }
        }

        private void UpdateName()
        {
            List<int> removeList = new List<int>();
            for (int i = 0; i < Subscribers.Count; i++)
            {
                var subscriber = Subscribers[i];
                if(subscriber == null) removeList.Add(i);
                else subscriber.UpdateKey(Name);
            }
            
            for (int i = removeList.Count-1; i > 0; i++)
            {
                Subscribers.RemoveAt(removeList[i]);
            }
        }

        public void ClearSubscribers()
        {
            Subscribers.Clear();
        }
    }
}