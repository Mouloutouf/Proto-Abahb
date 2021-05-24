/*
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities.Wireplug.Triggers
{
    public class InputTrigger : TriggerComponent
    {
        [SerializeField, FoldoutGroup("Triggers", order: 1)] private InputAction triggerAction;
        [ReadOnly] protected override bool MultiTrigger => !toggleable;

        [Space]
        [SerializeField, FoldoutGroup("Settings")]
        private bool toggleable;
        
        private void Start()
        {
            triggerAction.performed += OnPerformed;
            triggerAction.canceled += OnCanceled;
        }

        private void OnEnable()
        {
            triggerAction.Enable();
        }

        private void OnDisable()
        {
            triggerAction.Disable();
        }

        private void OnPerformed(InputAction.CallbackContext ctx)
        {
            Trigger();
        }
        
        private void OnCanceled(InputAction.CallbackContext ctx)
        {
            Untrigger();
        }
    }
}*/