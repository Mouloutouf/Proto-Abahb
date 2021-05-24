using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Wireplug.Triggers
{
    public class CallbackTrigger : TriggerComponent
    {
        [SerializeField, FoldoutGroup("Triggers", order:1)]
        private CallbackType triggerOn;
        [SerializeField, ShowIf("toggleable"), FoldoutGroup("Triggers")]
        private CallbackType untriggerOn;

        [ReadOnly] protected override bool MultiTrigger => !toggleable;

        [Space]
        [SerializeField, FoldoutGroup("Settings")]
        private bool toggleable;
        
        protected override void Awake()
        {
            multiTrigger = !toggleable;
            base.Awake();
            CheckTrigger(CallbackType.Awake);
        }

        private void Start()
        {
            CheckTrigger(CallbackType.Start);
        }

        private void OnEnable()
        {
            CheckTrigger(CallbackType.OnEnable);
        }

        private void OnDisable()
        {
            CheckTrigger(CallbackType.OnDisable);
        }

        private void OnDestroy()
        {
            CheckTrigger(CallbackType.OnDestroy);
        }

        private void CheckTrigger(CallbackType callback)
        {
            if (triggerOn.HasFlag(callback)) Trigger();
            if (toggleable && untriggerOn.HasFlag(callback)) Untrigger();
        }

        [Flags]
        private enum CallbackType
        {
            Awake = 1,
            Start = 2,
            OnEnable = 4,
            OnDisable = 8,
            OnDestroy = 16,
        }
    }
}
