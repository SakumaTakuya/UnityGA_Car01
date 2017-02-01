using System;
using UnityEngine;


namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
	public class MyStandaloneInput : VirtualInput
	{
		public override float GetAxis(string name, bool raw)
		{
			float inputValue = raw ? Input.GetAxisRaw (name) : Input.GetAxis (name);

			if (AxisExists(name)) {
				float customValue = raw ? VirtualAxisReference (name).GetValueRaw : VirtualAxisReference (name).GetValue;
				inputValue = (Mathf.Abs (inputValue) > Mathf.Abs (customValue)) ? inputValue : customValue;
			}

			return inputValue;
		}


		public override bool GetButton(string name)
		{
			bool isON = Input.GetButton (name);
			if (ButtonExists (name)) {
				isON |= VirtualButtonReference(name).GetButton;
			}
				
			return isON;
		}


		public override bool GetButtonDown(string name)
		{
			bool isON = Input.GetButtonDown (name);
			if (ButtonExists (name)) {
				isON |= VirtualButtonReference(name).GetButtonDown;
			}

			return isON;
		}


		public override bool GetButtonUp(string name)
		{
			bool isON = Input.GetButtonUp (name);
			if (ButtonExists (name)) {
				isON |= VirtualButtonReference(name).GetButtonUp;
			}

			return isON;
		}


		public override void SetButtonDown(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetButtonUp(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxisPositive(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxisNegative(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxisZero(string name)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override void SetAxis(string name, float value)
		{
			throw new Exception(
				" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}


		public override Vector3 MousePosition()
		{
			return Input.mousePosition;
		}
	}
}