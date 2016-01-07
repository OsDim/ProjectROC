using UnityEngine;
using System.Collections;

public class enemyCode : MonoBehaviour {

	public Transform target;
	public GameObject oddWay;
	public GameObject evenWay;

	public Transform spawnPos;

	public bool needAtk = false;
	public bool canAtk = false;

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

	//private Animator anim;
	//AnimatorStateInfo stateInfo;
	//int idleStateHash = Animator.StringToHash("Base Layer.rabbit_idle");


	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		//groundCheck = transform.Find("riceGroundCheck");
		//anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D> ().velocity = new Vector2 (-maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		atkCooldown = 0f;
		atkTimeCooldown = atkTime;
	}

	// Update is called once per frame
	void Update () {
		//stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		//anim.SetBool ("riceGround", Grounded);

		if (atkCooldown > 0) {
			atkCooldown -= Time.deltaTime;
		}

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
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void TakeHurt(int Direction){
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
			//anim.SetBool ("rabbitGround", Grounded);
			//}
		}
	}

	void OnTriggerExit2D(Collider2D colEx){
		if (colEx.gameObject.tag == "ground") {
			//if (player != null) {
			//anim.SetBool ("rabbitGround", Grounded);
			//}
		}
	}

	IEnumerator atkHit(){
		continuedTime = 0;
		atkOn = false;
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
		yield break;
	}
}
