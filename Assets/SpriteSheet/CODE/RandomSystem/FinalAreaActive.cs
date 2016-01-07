using UnityEngine;
using System.Collections;

public class FinalAreaActive : MonoBehaviour {

	public GameObject [] FinalArea = new GameObject[3];
	/*
	 * 0=主攝影機 
	 * 1=小怪隨機產生
	 * 2=王產生
	*/

	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Player") {
			if (FinalArea [0] != null) {
				if (FinalArea [0].GetComponent<cameraScript> () != null && FinalArea [0].GetComponent<cameraFollowPlayer> () != null) {
					FinalArea [0].GetComponent<cameraScript> ().enabled = false;
					FinalArea [0].GetComponent<cameraFollowPlayer> ().enabled = true;
					print ("CameraSwitch");
				}
			}
			if (FinalArea [1] != null) {
				FinalArea [1].SetActive (true);
			}
			if (FinalArea [2] != null) {
				FinalArea [2].SetActive (true);
			}
			this.gameObject.GetComponent<FinalAreaActive> ().enabled = false;
			//GetComponent<Rigidbody2D> ().position = new Vector2 (spawnPos.transform.position.x, spawnPos.transform.position.y);
		}
	}

	/*
	// Update is called once per frame
	void Update () {
	
	}
	*/
}
