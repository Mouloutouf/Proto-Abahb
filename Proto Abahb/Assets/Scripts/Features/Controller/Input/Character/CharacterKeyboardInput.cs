using UnityEngine;

namespace Controller.Input.Character
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterKeyboardInput : CharacterInput
    {
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Space;

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;

        public override float HorizontalInput()
		{
			if(useRawInput)
				return UnityEngine.Input.GetAxisRaw(horizontalInputAxis);
			else
				return UnityEngine.Input.GetAxis(horizontalInputAxis);
		}

		public override float VerticalInput()
		{
			if(useRawInput)
				return UnityEngine.Input.GetAxisRaw(verticalInputAxis);
			else
				return UnityEngine.Input.GetAxis(verticalInputAxis);
		}

		public override bool IsJumpKeyPressed()
		{
			return UnityEngine.Input.GetKey(jumpKey);
		}
    }
}
