using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
	[SerializeField]
	private DummyVirtualAxis axis;

	bool isPlay = false;
	bool isFinish = false;
	float speed = 0.0f;
	float distance = 0.0f;
	float resultDistance = 0.0f;
	float underLimitSpeed = 5.0f;
	float lowSpeedLimitTime = 5.0f;
	IEnumerator<bool> player;

	public bool IsFinished {
		get { return !isPlay && isFinish; }
	}

	// Use this for initialization
	void Start () {
		
	}

	public void Play(List<Param> paramList) {
		isPlay = true;
		isFinish = false;
		player = GetPlayer (paramList);
		resultDistance = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPlay)
			return;

		player.MoveNext ();
		if (player.Current)
			isPlay = false;
	}

	public void SetCrash() {
		isFinish = true;
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public void SetDistance(float distance) {
		this.distance = distance;
	}

	public float getPoint() {
		return resultDistance;
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

				if (speed <= underLimitSpeed) {
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

		resultDistance = distance;

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
