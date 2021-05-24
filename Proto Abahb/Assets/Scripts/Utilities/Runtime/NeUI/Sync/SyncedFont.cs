using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Runtime.NeUI.Sync
{
    public class SyncedFont : SyncedComponent<Font>
    {
        [SerializeField, PropertyOrder(-10), OnValueChanged("RecordOriginalValue")]
        private Text target;
        protected override string databaseName => "Library";
        protected override string keyName => "Font Name";
        protected override string valueName => "Font";

        protected override void Reset()
        {
            base.Reset();
            TryGetComponent(out target);
        }
        
        public override void UpdateTargetValue()
        {
            if (target != null) target.font = value;
        }

        public override Font GetValue()
        {
            return target != null ? target.font : null;
        }
    }
}