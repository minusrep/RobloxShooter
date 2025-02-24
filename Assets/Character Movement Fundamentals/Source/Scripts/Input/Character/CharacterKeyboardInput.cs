using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterKeyboardInput : CharacterInput
    {
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Space;

        public VariableJoystick _joystikMove;

        public bool useJoyStik;
        //If this is enabled, Unity's internal input smoothing is bypassed;
        public bool useRawInput = true;

        public bool isJump;
        public override float GetHorizontalMovementInput()
        {
            if (useJoyStik)
            {
                if (_joystikMove.Horizontal > 0.1f)
                {
                    return 1f;
                }
                else
                if (_joystikMove.Horizontal < -0.1f)
                {
                    return -1f;
                }
                else
                {
                    return 0f;
                }
            }
            else if (useRawInput)
                return Input.GetAxisRaw(horizontalInputAxis);
            else
                return Input.GetAxis(horizontalInputAxis);
        }

        public override float GetVerticalMovementInput()
        {
            if (useJoyStik)
            {
                if (_joystikMove.Vertical > 0.1f)
                {
                    return 1f;
                }
                else
                if (_joystikMove.Vertical < -0.1f)
                {
                    return -1f;
                }
                else
                {
                    return 0f;
                }
            }
            else if (useRawInput)
                return Input.GetAxisRaw(verticalInputAxis);
            else
                return Input.GetAxis(verticalInputAxis);
        }

        public override bool IsJumpKeyPressed()
		{
            if (useJoyStik)
                return isJump;
            else
            {
                return Input.GetKey(jumpKey);
            }
		}
    }
}
