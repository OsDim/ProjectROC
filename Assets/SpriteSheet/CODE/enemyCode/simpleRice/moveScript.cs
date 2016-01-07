using UnityEngine;
using System.Collections;

public class moveScript : MonoBehaviour {

	public float moveForce = 365f;			
	public float maxSpeed = 1f;			
	public float jumpForce = 650f;
	public float wallJumpForce = 500f;
	public float wallJpDelaytime =0.3f;
	public bool isJumpRice = false;
	public bool stopMove = false;
	public bool canHurt = false;
	public int hurtVector = 0;
	public float DamageCoolTime = 0.5f;

	/// Cooldown in seconds between two shots
	/// 冷卻
	public float jumpRate = 2f;
	//--------------------------------
	// 2 - Cooldown
	private float jumpCooldown;

	public bool facingRight = false;
	//true向左走,false向右走
	public bool rOrL = true;
	
	private Transform groundCheck;
	public bool Grounded = false;
	
	private Animator anim;


	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		groundCheck = transform.Find("riceGroundCheck");
		anim = GetComponent<Animator>();
	}
	
	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D> ().velocity = new Vector2 (-maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		jumpCooldown = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (groundCheck != null) {
			//Grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		}
		
		//anim.SetBool ("riceGround", Grounded);

		if (isJumpRice) {
			if (Input.GetButtonDown ("Jump") && Grounded) {
				if (CanJump){
					Jump();
				}
			}
		}

		if (jumpCooldown > 0)
		{
			jumpCooldown -= Time.deltaTime;
		}
	}

	void Jump(){
		anim.SetTrigger("Jump");
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce));
		jumpCooldown = jumpRate;
	}

	void FixedUpdate() {
		if (stopMove) {
			if(canHurt){
			TakeHurt(hurtVector);
			}
			canHurt = false;
		} else {
			canHurt = true;
			//當怪物向右時使其保持向右的速度，向左時保持向左的速度=Start======================================================================
			if (rOrL) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			} else {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			}
			//當怪物向右時使其保持向右的速度，向左時保持向左的速度=End======================================================================

			//翻轉圖片使其面向前進方向=Start=======================================================================================
			if (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) > 0f && !facingRight) {
				// ... flip the player.翻轉人物(向右)讓facingRight = true
				Flip ();
			}
		// Otherwise if the input is moving the player left and the player is facing right...
		//如果人物向左移動而人物面向右邊
			else if (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) < 0f && facingRight) {
				// ... flip the player.翻轉人物使其向左 讓facingRight = fales
				Flip ();
			}
			//翻轉圖片使其面向前進方向=End=======================================================================================
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			stopMove = !stopMove;
		}

	}

	public bool CanJump
	{
		get
		{
			return jumpCooldown <= 0f;
		}
	}

	void Flip (){
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void TakeHurt(int Direction){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);

		if (Direction == 1) {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(60,100), 100));
		} else {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-Random.Range(60,100), 100));
		}
		StartCoroutine(getHurt (DamageCoolTime));
		// Create a vector that's from the enemy to the player with an upwards boost.

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "CheckWall") {
			if (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) > 0f) {
				rOrL = true;
			}else{
				rOrL = false;
			}
		}

		if (col.gameObject.tag == "ground") {
			//if (player != null) {
				Grounded = true;
			anim.SetBool ("riceGround", Grounded);
			//}
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			//if (player != null) {
				Grounded = true;
			anim.SetBool ("riceGround", Grounded);
			//}
		}
	}
	
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			//if (player != null) {
				Grounded = false;
			anim.SetBool ("riceGround", Grounded);
			//}
		}
	}

	//無敵閃爍
	IEnumerator getHurt(float waitTime) {
		float t = 0;
		while (t < waitTime) {
			GetComponent<SpriteRenderer> ().color = Color.red;
			yield return new WaitForSeconds (0.15f);
			GetComponent<SpriteRenderer> ().color = Color.white;
			yield return new WaitForSeconds (0.05f);
			t += 0.1f;
			stopMove = false;
		}

		GetComponent<SpriteRenderer> ().color = Color.white;
	}
	
}
