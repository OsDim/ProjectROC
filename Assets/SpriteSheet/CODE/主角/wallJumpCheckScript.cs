using UnityEngine;
using System.Collections;

public class wallJumpCheckScript : MonoBehaviour {

	public GameObject player;
	private playerC playerSc;
	//private bool wallJumped;
	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		playerSc = player.GetComponent<playerC> ();
		//player = GameObject.Find ("Player").GetComponent<playerC> ();
	}
	
	void OnTriggerEnter2D(Collider2D col){
		
		if (playerSc != null) {
			if (playerSc.Grounded == false){
				if (col.gameObject.tag == "Wall" && playerSc.keyIN != false) {
					playerSc.wallJumped = true;
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D col){

		if (playerSc != null) {
			if (playerSc.Grounded == true){
				if (col.gameObject.tag == "Wall") {
					playerSc.wallJumped = false;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Wall") {
			if (playerSc != null) {
				playerSc.wallJumped = false;
			}
		}
	}
}
