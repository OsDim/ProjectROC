using UnityEngine;
using System.Collections;

public class rabbitMove : MonoBehaviour {
	
	public float moveForce = 365f;
	public float maxSpeed = 5f;			
	public float jumpForce = 650f;
	public float wallJumpForce = 500f;
	public float wallJpDelaytime =0.3f;
	public bool stopMove = false;
	public int hurtVector = 0;
	public float DamageCoolTime = 0.5f;

	public Transform player;
	public bool needAtk = false;
	public bool canAtk = false;
	public bool facingRight = false;
	//true向左走,false向右走
	public bool Grounded = false;
	
	/// Cooldown in seconds between two shots
	/// 冷卻
	public float atkRate = 5f;
	public float atkTime = 2.5f;
	//--------------------------------
	// 2 - Cooldown
	public float atkCooldown;
	public float atkTimeCooldown;
	public float continuedTime;

	//攻擊狀態
	public bool atkOn = false;

	private Animator anim;
	AnimatorStateInfo stateInfo;
	//int idleStateHash = Animator.StringToHash("Base Layer.rabbit_idle");
	
	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		//groundCheck = transform.Find("riceGroundCheck");
		anim = GetComponent<Animator>();
	}
	
	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D> ().velocity = new Vector2 (-maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		atkCooldown = 0f;
		atkTimeCooldown = atkTime;
	}
	
	// Update is called once per frame
	void Update () {
		stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		if (player != null && stateInfo.IsName ("rabbit_idle")) {
			if (player.position.x > gameObject.transform.position.x && !facingRight) {
				// ... flip the player.翻轉人物(向右)讓facingRight = true
				Flip ();
				facingRight = true;
			}else if (player.position.x < gameObject.transform.position.x && facingRight) {
				// ... flip the player.翻轉人物使其向左 讓facingRight = fales
				Flip ();
				facingRight = false;
			}
		}

		if (needAtk && CanAtk) {
			anim.SetBool("AtkHit", false);
			anim.SetTrigger("AtkTrigger");
			atkOn = true;
			//StartCoroutine(atkIng (atkTime));
			atkCooldown = atkRate;
			continuedTime = atkTime;
			anim.SetBool("AtkNoHit", false);
		}

		if (atkOn && continuedTime > 0) {
			continuedTime -= Time.deltaTime;
			if (facingRight) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			} else {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (-maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			}
		} else {
			atkOn = false;
			anim.SetBool("AtkNoHit", true);
		}

		//anim.SetBool ("riceGround", Grounded);
		
		if (atkCooldown > 0) {
			atkCooldown -= Time.deltaTime;
		}

	}
	
	void Jump(){
		anim.SetTrigger("Jump");
		GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce));
		atkCooldown = atkRate;
	}
	
	void FixedUpdate() {
	}
	
	public bool CanAtk
	{
		get
		{
			return atkCooldown <= 0f;
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
	
	public void TakeHurt(int Direction){
		if (stateInfo.IsName ("rabbit_idle")){
			print ("takeHurt");
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
			if (Direction == 1) {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (300, 400), 200));
			} else {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-Random.Range (300, 400), 200));
			}
		}
		//StartCoroutine(getHurt (DamageCoolTime));
		// Create a vector that's from the enemy to the player with an upwards boost.
		
		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
	}
	
	void OnTriggerEnter2D(Collider2D colEn){
		/*if (colEn.gameObject.tag == "CheckWall") {
			print(colEn.name);
			StartCoroutine (atkHit());
		}*/
		
		if (colEn.gameObject.tag == "ground") {
			//if (player != null) {
			Grounded = true;
			//anim.SetBool ("rabbitGround", Grounded);
			//}
		}
	}

	void OnCollisionEnter2D(Collision2D colionEn){
		if (colionEn.gameObject.tag == "Player") {
			print(colionEn.gameObject.name);
			StartCoroutine (atkHit());
		}
	}

	void OnTriggerStay2D(Collider2D colS){
		if (colS.gameObject.tag == "ground") {
			//if (player != null) {
			Grounded = true;
			//anim.SetBool ("rabbitGround", Grounded);
			//}
		}
	}
	
	void OnTriggerExit2D(Collider2D colEx){
		if (colEx.gameObject.tag == "ground") {
			//if (player != null) {
			Grounded = false;
			//anim.SetBool ("rabbitGround", Grounded);
			//}
		}
	}

	IEnumerator atkHit(){
		continuedTime = 0;
		atkOn = false;
		anim.SetBool("AtkHit", true);
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);

		if (facingRight) {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-Random.Range (300, 400),500));
		} else {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range (300, 400), 500));
		}
		yield break;
	}
}
