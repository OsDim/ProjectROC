using UnityEngine;
using System.Collections;

public class TaiwanBearAI : MonoBehaviour {

	public float moveForce = 365f;
	public float maxSpeed = 5f;			
	public float jumpForce = 650f;
	public int hurtVector = 0;
	public float DamageCoolTime = 0.5f;

	public Transform player;
	public bool needAtk = false;
	public bool canAtk = false;
	public bool facingRight = false;
	//true向左走,false向右走
	public bool Grounded = false;
	public bool playEff = true;
	public GameObject GroundEff;

	/// Cooldown in seconds between two shots
	/// 冷卻
	public float atkRate = 5f;
	public float atkTime = 2.5f;
	//--------------------------------
	// 2 - Cooldown
	public float atkCooldown;
	public float atkTimeCooldown;

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
		atkCooldown = atkRate;
		atkTimeCooldown = atkTime;
		if (facingRight) {
			Flip ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (Grounded && playEff) {
			playEff = false;
			if (GroundEff != null)
				StartCoroutine (endEff (0.85f));
		}
		stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		if (player != null && stateInfo.IsName ("idle")) {
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
			anim.SetTrigger("Atk");
			atkCooldown = Random.Range(3,atkRate);
		}

		//anim.SetBool ("riceGround", Grounded);

		if (atkCooldown > 0) {
			atkCooldown -= Time.deltaTime;
		}
	}

	//淡入
	IEnumerator endEff(float waitTime){
		GroundEff.SetActive (true);
		yield return new WaitForSeconds (waitTime);
		Destroy (GroundEff);
	}

	public bool CanAtk
	{
		get
		{
			return atkCooldown <= 0f;
		}
	}

	void Flip (){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D colEn){
		
		if (colEn.gameObject.tag == "ground") {
			Grounded = true;
		}
	}

	void OnTriggerStay2D(Collider2D colS){
		if (colS.gameObject.tag == "ground") {
			Grounded = true;
		}
	}

	void OnTriggerExit2D(Collider2D colEx){
		if (colEx.gameObject.tag == "ground") {
			Grounded = false;
		}
	}
}
