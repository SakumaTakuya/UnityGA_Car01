using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;


public class GAController : MonoBehaviour {
	
	[SerializeField]
	private CarController controller;

	List<Gene> geneList = new List<Gene>();
	bool isStart = false;

	IEnumerator<bool> player;

	int childNum = 20;
	int keepNum = 5;
	float mutateRate = 0.01f;

	public event Action<int, List<Gene>> onResultEvent = delegate {};

	// Use this for initialization
	void Start () {
		foreach (var i in Enumerable.Range(0, childNum)) {
			geneList.Add(new Gene());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Submit")) {
			isStart = true;
			player = GetPlayer (geneList);
		}

		if (isStart) {
			player.MoveNext ();
			if (player.Current) {
				isStart = false;
			}
		}
	}

	IEnumerator<bool> GetPlayer(List<Gene> geneList) {
		int generation = 0;
		string logDirPath = string.Format ("Log/{0}", DateTime.Now.ToString ("yyyyMMdd_hhmmss"));

		while (true) {
			float totalPoint = 0.0f;
			foreach (var gene in geneList) {
				controller.Play (gene.paramList);
				while (!controller.IsFinished) {
					yield return false;
				}

				gene.point = Mathf.Max(controller.getPoint (), 0.0f);
				totalPoint += gene.point;
			}

			foreach (var gene in geneList) {
				gene.rate = gene.point / totalPoint;
			}

			geneList.Sort ((a, b) => {
				if (a.point > b.point) return -1;
				else if (a.point < b.point) return 1;
				else return 0;
			});

			foreach (var gene in geneList) {
				Debug.Log (string.Format ("{0}/{1}", gene.point, gene.rate));
			}
			Debug.Log (string.Format ("=== Gen {0} : Top {1} m ===", generation, geneList [0].point));
			onResultEvent (generation, geneList);
			writeLog (logDirPath, generation, geneList);

			generation++;

			var newGeneList = new List<Gene> ();
			foreach (var i in Enumerable.Range(0, childNum)) {
				if (i < keepNum) {
					newGeneList.Add (geneList [i]);
				} else {
					var dad = SelectParent (geneList);
					var mom = SelectParent (geneList);
					var child = Mate (dad, mom);
					Mutate (ref child);
					newGeneList.Add (child);
				}
			}

			geneList = newGeneList;
		}
	}

	private Gene SelectParent(List<Gene> geneList) {
		float val = UnityEngine.Random.value;
		float totalRate = 0.0f;
		foreach (var gene in geneList) {
			totalRate += gene.rate;
			if (totalRate >= val) { 
				return gene;
			}
		}
		return geneList [geneList.Count - 1];
	}

	private Gene Mate(Gene dad, Gene mom) {
		Gene child = new Gene ();
		child.paramList.Clear ();

		int dadLimit = UnityEngine.Random.Range (1, dad.paramList.Count);
		int momLimit = UnityEngine.Random.Range (0, mom.paramList.Count);

		child.paramList = new List<CarController.Param> (
			dad.paramList.Skip (0).Take (dadLimit));
		child.paramList.AddRange (
			mom.paramList.Skip (momLimit).Take (mom.paramList.Count - momLimit));

		return child;
	}

	private void Mutate(ref Gene gene) {
		foreach (var i in Enumerable.Range(0, gene.paramList.Count)) {
			if (UnityEngine.Random.value < mutateRate) {
				gene.paramList[i] = CarController.Param.CreateRandom();
			}
		}
	}

	private void writeLog(string logDirPath, int generation, List<Gene> geneList) {
		if (!Directory.Exists (logDirPath)) Directory.CreateDirectory (logDirPath);

		string logPath = string.Format ("{0}/{1}.csv", logDirPath, generation);
		StreamWriter sw = new StreamWriter (logPath, false);
		foreach (var gene in geneList) {
			string str = string.Format ("{0},{1},{2}", gene.point, gene.rate, gene.paramList.Count);
			foreach (var param in gene.paramList) {
				str += "," + param.getString ();
			}
			sw.WriteLine (str);
		}
		sw.Flush ();
		sw.Close ();
	}

	public class Gene {
		public List<CarController.Param> paramList;
		public float rate;
		public float point;

		public Gene() {
			paramList = new List<CarController.Param>();
			for (int j = 0; j < UnityEngine.Random.Range(1, 11); j++) {
				paramList.Add(CarController.Param.CreateRandom());
			}
		}
	}
}