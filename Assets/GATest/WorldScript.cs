using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

using System;

public class WorldScript : MonoBehaviour {
	[SerializeField]
	private GameObject car;

	[SerializeField]
	private CarController controller;

	[SerializeField]
	private GAController ga;

	[SerializeField]
	private GUIScript gui;


	private float timeScale = 5.0f;

	// Use this for initialization
	void Start () {
		Debug.Log ("Start The World");
		InvokeRepeating ("sendParam", 0.0f, 0.033333f);

		ga.onResultEvent += (int generation, List<GAController.Gene> geneList) => {
			gui.setResult(string.Format("Gen:{0} Top:{1}", generation, geneList[0].point));
		};
	}
	
	// Update is called once per frame
	void Update () {
		Time.timeScale = timeScale;

		if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
			resetGame ();
		}
	}

	private void resetGame() {
		GameObject[] cars = GameObject.FindGameObjectsWithTag ("Player");
		foreach (var car in cars) {
			Destroy (car);
		}

		var obj = Instantiate (car);
		CarScript s = obj.GetComponent<CarScript> ();
		s.transform.position = GameObject.FindGameObjectWithTag ("Respawn").transform.position;
		s.CollidedWithWall += () => {
			controller.SetCrash();
		};

	}

	private void sendParam() {
		sendSpeed ();
		sendDistance ();
	}

	private void sendSpeed() {
		GameObject carobj = GameObject.FindGameObjectWithTag ("Player");
		if (carobj == null) return;

		float speed = carobj.GetComponent<CarScript> ().getSpeed ();
		controller.SetSpeed (speed);
	}

	private void sendDistance() {
		GameObject carobj = GameObject.FindGameObjectWithTag ("Player");
		if (carobj == null) return;

		float distance = carobj.GetComponent<CarScript> ().getDistance ();
		controller.SetDistance (distance);
	}
}
