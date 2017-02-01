using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;


public class DummyVirtualAxis : MonoBehaviour {

	private CrossPlatformInputManager.VirtualAxis m_HAxis;
	private CrossPlatformInputManager.VirtualAxis m_VAxis;
	private string m_HAxisName = "Horizontal";
	private string m_VAxisName = "Vertical"; 

	private CrossPlatformInputManager.VirtualButton m_ResetButton;
	private string m_ResetButtonName = "Cancel";

	[SerializeField]
	private float hSpeed = 3.0f;
	[SerializeField]
	private float vSpeed = 3.0f;

	private bool isLeftON = false;
	private bool isRightON = false;
	private bool isUpON = false;
	private bool isDownON = false;
	private bool isResetON = false;

	public bool isLeftBtnON {
		get { return isLeftON; }
	}
	public bool isRightBtnON {
		get { return isRightON; }
	}
	public bool isUpBtnON {
		get { return isUpON; }
	}
	public bool isDownBtnON {
		get { return isDownON; }
	}

	// Use this for initialization
	void Start () {
		ResetAxis (ref m_HAxis, m_HAxisName);
		ResetAxis (ref m_VAxis, m_VAxisName);
		ResetButton (ref m_ResetButton, m_ResetButtonName);
	}

	private void ResetAxis(ref CrossPlatformInputManager.VirtualAxis axis, string axisName) {
		if (!CrossPlatformInputManager.AxisExists (axisName)) {
			// if the axis doesnt exist create a new one in cross platform input
			axis = new CrossPlatformInputManager.VirtualAxis (axisName, false);
			CrossPlatformInputManager.RegisterVirtualAxis (axis);
		} else {
			axis = CrossPlatformInputManager.VirtualAxisReference (axisName);
		}
	}

	private void ResetButton(ref CrossPlatformInputManager.VirtualButton button, string buttonName) {
		if (!CrossPlatformInputManager.ButtonExists(buttonName)) {
			button = new CrossPlatformInputManager.VirtualButton (buttonName);
			CrossPlatformInputManager.RegisterVirtualButton(button);
		} else {
			button = CrossPlatformInputManager.VirtualButtonReference(buttonName);
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateAxis(ref m_HAxis, isLeftON, isRightON, hSpeed);
		UpdateAxis (ref m_VAxis, isDownON, isUpON, vSpeed);

		if (isResetON) {
			m_ResetButton.Pressed ();
			m_ResetButton.Released ();

			isResetON = false;
			isLeftON = false;
			isRightON = false;
			isUpON = false;
			isDownON = false;
		}
	}

	private void UpdateAxis(ref CrossPlatformInputManager.VirtualAxis axis, bool isNegativeON, bool isPositiveON, float speed) {
		if (axis == null) return;

		float refValue = 0;
		refValue += isNegativeON ? -1 : 0;
		refValue += isPositiveON ? 1 : 0;

		axis.Update (Mathf.MoveTowards (axis.GetValue, refValue, speed * Time.deltaTime));
	}

	public void TriggerLeft(bool isON) {
		isLeftON = isON;
	}

	public void TriggerRight(bool isON) {
		isRightON = isON;
	}

	public void TriggerGo(bool isON) {
		isUpON = isON;
	}

	public void TriggerBack(bool isON) {
		isDownON = isON;
	}

	public void TriggerReset(bool isON) {
		isResetON = isON;
	}
}
