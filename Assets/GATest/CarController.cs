using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;


public class CarController : MonoBehaviour {
	[SerializeField]
	private DummyVirtualAxis axis;

	[SerializeField]
	private GameObject carTemplate;
	private CarScript car;

	[SerializeField]
	private GUIScript gui;

	bool isPlay = false;
	bool isFinish = false;
	float resultDistance = 0.0f;
	float underLimitSpeed = 5.0f;
	float lowSpeedLimitTime = 5.0f;
	IEnumerator<bool> player;
	string result;


	public bool IsFinished {
		get { return !isPlay && isFinish; }
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("Start The World");
		Time.timeScale = 5.0f;
	}

	public void Play(List<Param> paramList) {
		isPlay = true;
		isFinish = false;
		player = GetPlayer (paramList);
		resultDistance = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
			resetGame ();
		}
	}

	void FixedUpdate () {
		if (!isPlay)
			return;

		player.MoveNext ();
		if (player.Current)
			isPlay = false;
	}

	public void SetCrash() {
		isFinish = true;
	}

	public float getPoint() {
		return resultDistance;
	}

	public void setResult(int generation, List<GAController.Gene> geneList) {
		result = string.Format("Gen:{0} Top:{1}", generation, geneList[0].point);
		gui.setResult (result);
	}

	private IEnumerator<bool> GetPlayer(List<Param> paramList) {
		float lowSpeedTime = 0.0f;

		axis.TriggerReset (true);
		yield return false;

		foreach (var time in yieldTimer(1.0f)) yield return false;

		while (!isFinish) {
			foreach (var param in paramList) {
				if (isFinish) break;

				axis.TriggerGo (param.isGo);
				axis.TriggerBack (param.isBack);
				axis.TriggerLeft (param.isLeft);
				axis.TriggerRight (param.isRight);

				foreach (var time in yieldTimer(0.5f)) yield return false;

				if (car.getSpeed() <= underLimitSpeed) {
					lowSpeedTime += 0.5f;
					if (lowSpeedTime >= lowSpeedLimitTime) {
						isFinish = true;
						break;
					}
				} else {
					lowSpeedTime = 0.0f;
				}
			}
		}

		resultDistance = car.getDistance();

		axis.TriggerGo (false);
		axis.TriggerBack (false);
		axis.TriggerLeft (false);
		axis.TriggerRight (false);
		foreach (var time in yieldTimer(1.0f)) yield return false;

		yield return true;
	}

	private IEnumerable<float> yieldTimer(float waitSec) {
		float time = 0.0f;
		while (time < waitSec) {
			yield return time;
			time += Time.deltaTime;
		}
	}

	private void resetGame() {
		GameObject[] cars = GameObject.FindGameObjectsWithTag ("Player");
		foreach (var car in cars) {
			Destroy (car);
		}

		var obj = Instantiate (carTemplate);
		car = obj.GetComponent<CarScript> ();
		car.transform.position = GameObject.FindGameObjectWithTag ("Respawn").transform.position;
		car.CollidedWithWall += () => {
			SetCrash();
		};
	}
			
	public class Param {
		public bool isGo;
		public bool isBack;
		public bool isLeft;
		public bool isRight;

		public Param() {}

		public Param(bool isGo, bool isBack, bool isLeft, bool isRight) {
			this.isGo = isGo;
			this.isBack = isBack;
			this.isLeft = isLeft;
			this.isRight = isRight;
		}

		static public Param CreateRandom() {
			var param = new Param ();
			param.isGo = (Random.Range (0, 2) == 0);
			param.isBack = (Random.Range (0, 2) == 0);
			param.isLeft = (Random.Range (0, 2) == 0);
			param.isRight = (Random.Range (0, 2) == 0);
			return param;
		}

		public string getString() {
			int val = (isGo ? 8 : 0) +
			          (isBack ? 4 : 0) +
			          (isLeft ? 2 : 0) +
			          (isRight ? 1 : 0);
			return val.ToString ("X");
		}
	}
}
