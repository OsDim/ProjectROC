using UnityEngine;
using System.Collections;

public class BossCowAI : MonoBehaviour {

	[HideInInspector]
	// For determining which way the player is currently facing.
	public bool facingRight = true;			
	public int face = -1;//面向右邊為1,左為-1			
	
	//[HideInInspector]
	public bool jump = false;
	//private bool wallJump = false;

	public Transform spawnPos;
	public GameObject PunchArea;

	public float moveForce = 365f;			
	public float maxSpeed = 100f;
	public float nSpeed = 1f;
	public float jumpForce = 650f;
	public float wallJumpForce = 500f;
	public float wallJpDelaytime =0.3f;
	
	//private Transform groundCheck;
	public bool Grounded = false;
	//private Transform wallJumpCheck;
	public bool wallJumped = false;

	//攻擊動作選擇
	public float atkChMax = 10f;
	public float atkDivide = 6f;//動作選擇分界，小於Divide動作為捶地
	public float atkChoice = 0f;
	public bool atkPunch = false;
	public bool atkRush = false;
	public bool atkActionIng = false;

	public bool atkHandHit = false;

	/// Cooldown in seconds between two shots
	/// 冷卻
	public float canAtk = 0f;
	public float atkRunRate = 5f;
	public float atkHandRate = 5f;

	public float canAtkCooldown = 3f;
	private float atkRunCooldown = 5f;
	private float atkHandCooldown = 5f;

	public bool runIng = false;
	public bool runStart = false;
	public bool handIng = false;

	private Animator anim;

	public AudioClip[] sound;
	AudioSource NewAudio;

	void Awake()
	{
		// Setting up references.
		//groundCheck = transform.Find("groundCheck");
		//wallJumpCheck = transform.Find("wallJumpCheck");
		spawnPos = GameObject.Find("rightSideWall").transform;
		canAtk = canAtkCooldown;
		anim = GetComponent<Animator>();
		NewAudio = GetComponent<AudioSource> ();
		NewAudio.PlayOneShot(sound[0],1f);
		StartCoroutine (BGMStart (1f));
	}

	// Use this for initialization
	void Start () {
		if (PunchArea != null) {
			PunchArea.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("atkRunIng", runIng);
		anim.SetBool ("atkRunStart", runStart);
		anim.SetBool ("atkHandIng", handIng);

		if (CanAtk) {
			if (atkActionIng == false) {
				atkChoice = Random.Range (0f, atkChMax);
				if (atkChoice < atkDivide) {
					atkPunch = true;
					atkRush = false;
				} else {
					if (CanAtkRun) {
						atkRush = true;
						atkPunch = false;
					} else {
						atkPunch = true;
						atkRush = false;
					}
				}
				atkActionIng = true;
			}
		}
			
		if (atkPunch) {
			atkHand();
		}else if (atkRush) {
			atkRun();
		}

		if ((runIng == false) && Grounded){
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);		
		}

		if (PunchArea != null) {
			if (atkHandHit) {
				PunchArea.SetActive (true);
			} else {
				PunchArea.SetActive (false);
			}
		}
			
		cooldown ();
	}

	//待機計數結束了嗎?
	public bool CanAtk
	{
		get	{
			return canAtk <= 0f;
		}
	}

	void cooldown(){
		if (atkRunCooldown > 0)	{
			atkRunCooldown -= Time.deltaTime;
		}

		if (atkHandCooldown > 0){
			atkHandCooldown -= Time.deltaTime;
		}

		if (canAtk > 0){
			canAtk -= Time.deltaTime;
		}
	}

	//衝刺攻擊
	void atkRun(){
		atkRush = false;
		NewAudio.PlayOneShot(sound[0],1f);
		anim.SetTrigger("atkRun");
		StartCoroutine (speedUP (10f));
	}

	//捶地攻擊
	void atkHand(){
		atkPunch = false;
		anim.SetTrigger("atkHand");
		StartCoroutine (HandStart (0.7f));
		canAtk = canAtkCooldown;
		//atkHandCooldown = atkHandRate;
		atkActionIng = false;
	}

	public bool CanAtkRun {
		get	{
			return atkRunCooldown <= 0f;
		}
	}

	public bool CanAtkHand{
		get {
			return atkHandCooldown <= 0f;
		}
	}
	//OnCollisionEnter2D
	//OnTriggerEnter2D
	void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.name == "leftSideWall") {
			GetComponent<Rigidbody2D> ().position = new Vector2 (spawnPos.transform.position.x, spawnPos.transform.position.y);
		}
	}

	void OnTriggerStay2D(Collider2D col){

		print ("switch");

		if (col.gameObject.tag == "Ground") {
			Grounded = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){

		if (col.gameObject.tag == "Ground") {
			Grounded = false;
		}
	}

	void runinggg(){
		runIng = true;
		anim.SetBool ("atkRunIng", runIng);
	}

	//衝刺加速
	IEnumerator speedUP(float waitTime){
		while (runStart == false) {
			canAtk = canAtkCooldown;
			yield return new WaitForSeconds (0.02f);
		}
		float i = 0;
		float j = 40;
		while (i < waitTime) {
			runinggg ();
			canAtk = canAtkCooldown;
			i += 0.2f;
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-((i * 3) + j), 0));
			yield return new WaitForSeconds (0.01f);
		}
		i = 0;
		while (i < (waitTime * 2)) {
			runinggg ();
			canAtk = canAtkCooldown;
			i += 0.1f;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			yield return new WaitForSeconds (0.01f);
		}

		runIng = false;
		anim.SetBool ("atkRunIng", runIng);
		atkRunCooldown = atkRunRate;
		canAtk = canAtkCooldown;
		atkActionIng = false;
	}

	//BGM
	IEnumerator BGMStart(float waitTime){
		yield return new WaitForSeconds (waitTime);
		NewAudio.PlayOneShot(sound[2],0.4f);
	}

	//Hand
	IEnumerator HandStart(float waitTime){
		yield return new WaitForSeconds (waitTime);
		NewAudio.PlayOneShot(sound[1],0.8f);
	}
}
