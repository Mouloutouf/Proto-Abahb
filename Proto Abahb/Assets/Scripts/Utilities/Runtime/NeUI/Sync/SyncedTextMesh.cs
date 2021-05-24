using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utilities.Runtime.NeUI.Sync
{
    public class SyncedTextMesh : SyncedComponent<TMP_FontAsset>
    {
        [SerializeField, PropertyOrder(-10), OnValueChanged("RecordOriginalValue")]
        private TextMeshProUGUI target;
        protected override string databaseName => "TextMesh Library";
        protected override string keyName => "Font Name";
        protected override string valueName => "Font";
        public override void UpdateTargetValue()
        {
            if (target != null) target.font = value;
        }

        public override TMP_FontAsset GetValue()
        {
            return target != null ? target.font : null;
        }
    }
}