using UnityEngine;
using System.Collections;

public class playerStopWallScript : MonoBehaviour {

	public Vector2 offset;

	private cameraScript cameraS;
	public Transform cameraPos;
	public float wallPos;

	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		cameraS = GameObject.Find ("mainCamera").GetComponent<cameraScript> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		wallPos = cameraPos.transform.position.x + offset.x;
		transform.position = new Vector2 (wallPos, transform.position.y); 

	}
}
