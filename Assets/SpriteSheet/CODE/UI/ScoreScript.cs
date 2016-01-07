using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {
	public int score = 0;

	private playerC playerControl;
	public GameObject ScoreText;
	private int previousScore = 0;


	void Awake(){

		playerControl = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerC> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		this.GetComponent<GUIText>().text = "ScorE:" + score;
		if (previousScore != score) {
			previousScore = score;
		}
	
	}
}
