using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Runtime.NeUI.Sync
{
    public class SyncedColor : SyncedComponent<Color>
    {
        [SerializeField, PropertyOrder(-10), OnValueChanged("RecordOriginalValue")]
        private Image target;
        protected override string databaseName => "Database";
        protected override string keyName => "Color Name";
        protected override string valueName => "Color";

        protected override void Reset()
        {
            base.Reset();
            TryGetComponent(out target);
        }
        
        public override void UpdateTargetValue()
        {
            if (target != null) target.color = this.value;
        }

        public override Color GetValue()
        {
            return target != null ? target.color : Color.black;
        }
    }
}