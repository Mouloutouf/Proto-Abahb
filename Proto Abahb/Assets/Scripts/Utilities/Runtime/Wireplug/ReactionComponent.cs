using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Wireplug;

namespace Utilities.Wireplug
{
    public abstract class ReactionComponent : MonoBehaviour
    {
        [Space, SerializeField, HideIf("needsAtLeast"), FoldoutGroup("Settings")]
        private bool needAllTriggers;  
        [SerializeField, HideIf("needAllTriggers"), FoldoutGroup("Settings")]
        private bool needsAtLeast;
        [SerializeField, ShowIf("needsAtLeast"), FoldoutGroup("Settings")]
        private int atLeast;
        [SerializeField, FoldoutGroup("Settings")]
        private bool delayed;
        [SerializeField, ShowIf("delayed"), FoldoutGroup("Settings")]
        private float delayDuration;
        [SerializeField, ShowIf("delayed"), FoldoutGroup("Settings")]
        private bool cancelOnUntrigger = true;

        private Dictionary<TriggerComponent, bool> triggerStates = new Dictionary<TriggerComponent, bool>();
        private bool on;

        private Coroutine delayedReaction;
        public void Link(TriggerComponent trigger)
        {
            triggerStates.Add(trigger, false);
        }

        public void Trigger(TriggerComponent trigger)
        {
            var wasOn = triggerStates[trigger];
            triggerStates[trigger] = true;
            if(!needAllTriggers || needsAtLeast) React();
            else if (!on && !wasOn)
            {
                if ((needAllTriggers && triggerStates.All(t => t.Value == true)) ||
                    (needsAtLeast && triggerStates.Count(t => t.Value == true) >= atLeast)) React();
            }
        }
        
        public void Untrigger(TriggerComponent trigger)
        {
            if(triggerStates[trigger] == false) return;
            triggerStates[trigger] = false;
            if (on)
            {
                var triggersOn = triggerStates.Count(t => t.Value == true);
                if (needsAtLeast && triggersOn < atLeast || needAllTriggers && triggersOn == triggerStates.Count || triggersOn == 0) Unreact();
            }
        }

        [Button]
        public void React()
        {
            on = true;
            if (delayed) delayedReaction = StartCoroutine(Delay());
            else OnReact();
        }

        [Button]
        public void Unreact()
        {
            on = false;
            if (delayed && cancelOnUntrigger && delayedReaction != null)
            {
                StopCoroutine(delayedReaction);
                delayedReaction = null;
            }
            OnUnreact();
        }
        
        public abstract void OnReact();

        public abstract void OnUnreact();

        
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(delayDuration);
            OnReact();
        }
    }
}
