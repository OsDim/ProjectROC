using UnityEngine;
using System.Collections;

public class PointUp : MonoBehaviour {

	public ScoreScript Score;
	public int point = 100;
	public bool End = false;

	// Use this for initialization
	void Awake () {
		Score = GameObject.Find("ScoreText").GetComponent<ScoreScript>();
		Score.score += point;
	
	}
	void Update () {
		if (End) {
			destroy ();
		}
	}
	
	// Update is called once per frame
	void destroy () {
		Destroy (gameObject);
	}

}
