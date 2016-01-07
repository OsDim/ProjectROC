using UnityEngine;
using System.Collections;

public class CameraYtop : MonoBehaviour {

	public GameObject MainCamera;
	private cameraScript CameraSc;


	// Use this for initialization
	void Start () {
		if (MainCamera != null) {
			CameraSc = MainCamera.GetComponent<cameraScript> ();
		}
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			CameraSc.moveToTop = true;
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			CameraSc.moveToTop = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			CameraSc.moveToTop = false;
		}
	}
}
