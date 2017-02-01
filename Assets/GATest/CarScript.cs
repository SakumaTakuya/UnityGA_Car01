using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarScript : MonoBehaviour {

	public event Action CollidedWithWall = delegate {};

	private float distance = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		distance += getSpeed () * Time.deltaTime;
	}

	public void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Wall") {
			CollidedWithWall ();
		}
	}

	public float getSpeed() {
		return Vector3.Dot(GetComponent<Rigidbody> ().velocity, gameObject.transform.forward);
	}

	public float getDistance() {
		return distance;
	}
}
