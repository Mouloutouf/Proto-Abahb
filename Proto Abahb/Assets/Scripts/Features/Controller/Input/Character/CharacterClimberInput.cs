using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controller.Input.Character
{
    public class CharacterClimberInput : CharacterInput
    {
        public string horizontalInputAxis = "Horizontal";
        public string verticalInputAxis = "Vertical";
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode dashKey = KeyCode.LeftControl;

        [NonSerialized, ShowInInspector]
        public DirectionalKeys directionalKeys = new DirectionalKeys();

        private void Update()
        {
            directionalKeys.Evaluate(VerticalInput(), HorizontalInput());
        }

        public override float HorizontalInput() => UnityEngine.Input.GetAxisRaw(horizontalInputAxis);

        public override float VerticalInput() => UnityEngine.Input.GetAxisRaw(verticalInputAxis);

        public override bool IsJumpKeyPressed() => UnityEngine.Input.GetKey(jumpKey);

        public bool IsDashKeyPressed() => UnityEngine.Input.GetKey(dashKey);
    }
}