using UnityEngine;
using System.Collections;

public class groundCheck : MonoBehaviour {

	private playerC player;
	private bool Grounded;


	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		player = GameObject.Find ("Player").GetComponent<playerC> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			if (player != null) {
				player.Grounded = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			if (player != null) {
				player.Grounded = true;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			if (player != null) {
				player.Grounded = false;
			}
		}
	}
}
