using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ULib.Scripts.Sensors
{
    public class RangeSensor : MonoBehaviour
    {
        #region Settings
        [TitleGroup("Settings"), SerializeField]
        private Vector3 offset;
        [TitleGroup("Settings"), SerializeField]
        private float range;
        [TitleGroup("Settings"), SerializeField]
        private bool useSight;
        [TitleGroup("Settings"), SerializeField]
        private LayerMask targetMask;
        [TitleGroup("Settings"), ShowIf("useSight"), SerializeField]
        private LayerMask sightMask;
    
        [FoldoutGroup("Debug"), ShowInInspector, HideInEditorMode, ReadOnly]
        private List<Collider> inRange = new List<Collider>();
        [FoldoutGroup("Debug"), ShowInInspector, HideInEditorMode, ReadOnly]
        private List<Collider> inSight = new List<Collider>();
        private Vector3 origin;
        #endregion
        // Update is called once per frame
        void Update()
        {
            origin = transform.position + offset;
            Sensor();

        }

        void Sensor()
        {
            inRange.Clear();
            inSight.Clear();
        
            inRange = Physics.OverlapSphere(origin, range, targetMask).ToList();
        
            for (int i = 0; i < inRange.Count; i++)
            {
                var result = inRange[i];
                if (useSight)
                {
                    if (Physics.Raycast(origin, result.transform.position - origin, out RaycastHit info, range,sightMask))
                    {
                        if (info.collider == result)
                        {
                            inSight.Add(result);
                        }
                    }
                }
            }

            Closest(true);
        }

        public Collider Closest(bool inSight)
        {
            var list = inSight && useSight ? this.inSight : inRange;
            if (list.Count > 0)
            {
                var closest = list.OrderBy(o => Vector3.Distance(o.ClosestPointOnBounds(origin), origin)).First();
            
                return closest;
            }
            return null;
        }
    }
}
