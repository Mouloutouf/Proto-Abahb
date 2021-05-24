using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Wireplug
{
    public abstract class TriggerComponent : MonoBehaviour
    {
        [SerializeField, FoldoutGroup("Settings", order: 3)]
        protected TriggerMode mode = TriggerMode.Event;

        [ShowInInspector, FoldoutGroup("Settings")]
        protected virtual bool MultiTrigger
        {
            get => multiTrigger;
            set => multiTrigger = value;
        }
        [SerializeField, HideInInspector]
        protected bool multiTrigger = false;
        [Space]
        [SerializeField, FoldoutGroup("@mode", order: 2), ShowIf("mode", TriggerMode.Reaction)]
        private List<ReactionComponent> reactions = new List<ReactionComponent>();
        [SerializeField, FoldoutGroup("@mode"), ShowIf("mode", TriggerMode.Event)] 
        private UnityEvent onTrigger = new UnityEvent();
        [SerializeField, FoldoutGroup("@mode"), ShowIf("@mode == TriggerMode.Event && !MultiTrigger")] 
        private UnityEvent onUntrigger = new UnityEvent();
        [SerializeField, FoldoutGroup("@mode"), ShowIf("mode", TriggerMode.SetActive)]
        private GameObject target = default;
        [SerializeField, FoldoutGroup("@mode"), ShowIf("@mode == TriggerMode.SetActive && !setDisabledOnTrigger")]
        private bool setActiveAsToggle = default;
        [SerializeField, FoldoutGroup("@mode"), ShowIf("@mode == TriggerMode.SetActive && !setActiveAsToggle")]
        private bool setDisabledOnTrigger = default;
        
        [ShowInInspector, HideInEditorMode, BoxGroup("Debug")]
        protected bool active = default;
        
        protected virtual void Awake()
        {
            if (mode == TriggerMode.Reaction)
            {
                foreach (var reaction in reactions)
                {
                    reaction?.Link(this);
                }
            }
        }

        public void Trigger()
        {
            if(active == true && !multiTrigger) return;
            active = true;
            switch (mode)
            {
                case TriggerMode.Reaction:
                    foreach (var reaction in reactions)
                    {
                        reaction?.Trigger(this);
                    }
                    break;
                case TriggerMode.Event:
                    onTrigger?.Invoke();
                    break;
                case TriggerMode.SetActive:
                    target.SetActive(setActiveAsToggle ? !target.activeSelf : !setDisabledOnTrigger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Untrigger()
        {
            if(active == false) return;
            active = false;
            switch (mode)
            {
                case TriggerMode.Reaction:
                    foreach (var reaction in reactions)
                    {
                        reaction?.Untrigger(this);
                    }
                    break;
                case TriggerMode.Event:
                    onUntrigger?.Invoke();
                    break;
                case TriggerMode.SetActive:
                    target.SetActive(setActiveAsToggle ? !target.activeSelf : setDisabledOnTrigger);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public enum TriggerMode
        {
            Reaction,
            Event,
            SetActive
        }
    }
}