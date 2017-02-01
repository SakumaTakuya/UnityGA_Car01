using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;


public class GUIScript : MonoBehaviour {
	[SerializeField]
	private DummyVirtualAxis axis;

	private bool isLON;
	private bool isRON;
	private bool isUON;
	private bool isDON;
	private float speed;
	private float distance;
	private LinkedList<string> resultList = new LinkedList<string>();

	private object guard = new object();

	void OnGUI() {
		var carObj = GameObject.FindGameObjectWithTag ("Player");
		if (carObj) {
			var carScriptObj = carObj.GetComponent<CarScript>();
			speed = carScriptObj.getSpeed ();
			distance = carScriptObj.getDistance ();
		}

		GUI.Label (new Rect (10, 10, 500, 100), 
			string.Format("Speed: {0}", speed.ToString()));

		GUI.Label (new Rect (10, 30, 500, 100),
			string.Format ("Distance: {0}", distance.ToString ()));

		GUI.Label (new Rect (10, 50, 500, 100),
			string.Format ("上下: {0}", CrossPlatformInputManager.GetAxis ("Vertical")));

		GUI.Label (new Rect(10, 70, 500, 100),
			string.Format("左右: {0}", CrossPlatformInputManager.GetAxis ("Horizontal")));

		GUI.Label (new Rect (10, 90, 500, 100),
			string.Format ("UP:{0}, DOWN:{1}, LEFT:{2}, RIGHT:{3}",
				axis.isUpBtnON ? "@" : "_", 
				axis.isDownBtnON ? "@" : "_",
				axis.isLeftBtnON ? "@" : "_",
				axis.isRightBtnON ? "@" : "_"));

		string str = "";
		lock (guard) {
			foreach (var result in resultList) {
				str += result + "\r\n";
			}
		}
		GUI.Label (new Rect(10, 110, 500, 500), str);
	}

	public void setResult(string str) {
		lock (guard) {
			resultList.AddFirst (str);
			if (resultList.Count > 30) {
				resultList.RemoveLast ();
			}
		}
	}
}
