using UnityEngine;
using System.Collections;

public class riceGroundCheckSprite : MonoBehaviour {
	
	private Transform groundCheck;
	public bool Grounded = false;

	public float jumpForce = 650f;

	private Animator anim;
	
	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}

	void Update(){
		Grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		anim.SetBool ("Grounded", Grounded);

		if (Input.GetButtonDown ("Jump") && Grounded) {
			Jump();
		}
	}

	void Jump(){
		anim.SetTrigger("Jump");
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce));
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
	//		if (player != null) {
	//			player.Grounded = true;
	//		}
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
	//		if (player != null) {
	//			player.Grounded = false;
	//		}
		}
	}
}
