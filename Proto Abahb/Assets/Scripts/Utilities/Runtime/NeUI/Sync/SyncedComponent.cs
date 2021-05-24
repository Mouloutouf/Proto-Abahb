using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Runtime.NeUI.Sync
{
    public abstract class SyncedComponent<T> : MonoBehaviour
    {
        [SerializeField, OnValueChanged("OnSettingsChanged"), LabelText("@databaseName")] 
        public SyncDatabase<T> database;
        [SerializeField, OnValueChanged("OnSettingsChanged"), LabelText("@keyName"), ValueDropdown("AvailableKeys")] 
        private string key;
        [SerializeField, ReadOnly]
        protected T value;

        protected abstract string databaseName { get; }
        protected abstract string keyName { get; }
        protected abstract string valueName { get; }

        private SyncDatabase<T> prevDatabase;
        private string prevKey;
        protected T originalValue;

        protected virtual void Reset()
        {
            RecordOriginalValue();
        }

        protected virtual void Start()
        {
            RefreshValue();
        }

        public void RefreshValue()
        {
            database.SubscribeTo(this, key);
        }

        public void UpdateValue(T value)
        {
            this.value = value;
            UpdateTargetValue();
        }
        
        public void UpdateKey(string key)
        {
            prevKey = this.key = key;
        }

        public abstract void UpdateTargetValue();

        public abstract T GetValue();

        private void ToDefault()
        {
            key = database.Default();
            database.SubscribeTo(this, key);
        }

        private void RecordOriginalValue()
        {
            originalValue = GetValue();
        }
        
        public void RevertValue()
        {
            UpdateValue(originalValue);
            key = null;
        }

        private string[] AvailableKeys()
        {
            if (database != null) return database.Keys().ToArray();
            else return new string[0]{};
        }
        
        private void OnSettingsChanged()
        {
            if (prevDatabase != database)
            {
                if (database != null) ToDefault();
                else RevertValue();
                if (prevDatabase != null && !string.IsNullOrEmpty(prevKey)) prevDatabase.UnsubscribeFrom(this, prevKey);
            }
            else if (prevKey != key)
            {
                if (key != null) database.SubscribeTo(this, key);
                else ToDefault();
                if (database != null && !string.IsNullOrEmpty(prevKey)) database.UnsubscribeFrom(this, prevKey);

            }
            prevDatabase = database;
            prevKey = key;
        }
    }
}