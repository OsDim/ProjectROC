using UnityEngine;
using System.Collections;

public class checkWallScript : MonoBehaviour {

	private cameraScript cameraS;

	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		cameraS = GameObject.Find ("mainCamera").GetComponent<cameraScript> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "cameraMovwPoint") {
			if (cameraS != null) {
				cameraS.cameraMove = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "cameraMovwPoint") {
			if (cameraS != null) {
				cameraS.cameraMove = true;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "cameraMovwPoint") {
			if (cameraS != null) {
				cameraS.cameraMove = false;
			}
		}
	}
}
