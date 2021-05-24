using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Wireplug.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class ZoneTrigger : TriggerComponent
    {
        [SerializeField, FoldoutGroup("Settings")] private LayerMask targetLayers;
        [SerializeField, FoldoutGroup("Settings"), HideIf("needsPrecisely")] private bool needsAtLeast;
        [SerializeField, FoldoutGroup("Settings"), HideIf("needsAtLeast")] private bool needsPrecisely;
        [SerializeField, ShowIf("@needsAtLeast || needsPrecisely"), FoldoutGroup("Settings")] private int needs;
        [ShowInInspector, HideInEditorMode, BoxGroup("Debug")]
        private List<GameObject> entities = new List<GameObject>();
        private void OnTriggerEnter(Collider other)
        {
            if (IsInLayerMask(gameObject, targetLayers))
            {
                RemoveDestroyedEntities();
                entities.Add(other.gameObject);
                if(!active && (!needsAtLeast || entities.Count >= needs) && (!needsPrecisely || entities.Count == needs)) Trigger();
                else if(needsPrecisely && entities.Count != needs) Untrigger();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsInLayerMask(gameObject, targetLayers))
            {
                if (entities.Contains(other.gameObject))
                {
                    entities.Remove(other.gameObject);
                }
                RemoveDestroyedEntities();

                if (needsAtLeast)
                {
                    if (entities.Count < needs) Untrigger();
                }
                else if (needsPrecisely)
                {
                    if(entities.Count != needs) Untrigger();
                    else Trigger();
                }
                else if (entities.Count == 0) Untrigger();
            }
        }

        private void RemoveDestroyedEntities()
        {
            for (int i = entities.Count-1; i >= 0; i--)
            {
                if(entities[i] == null) entities.RemoveAt(i);
            }
        }
        
        public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
    }
}