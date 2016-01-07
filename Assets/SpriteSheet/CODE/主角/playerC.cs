using UnityEngine;
using System.Collections;

public class playerC : MonoBehaviour {
	
	[HideInInspector]
	// For determining which way the player is currently facing.
	public bool facingRight = true;			
	public int face = 1;//面向右邊為1,左為-1
	public bool isMobile = false;
	public float ButtonH = 0;	
	public float showH = 0;
	public bool RButtonDown = false;
	public bool LButtonDown = false;

	//[HideInInspector]
	public bool jump = false;
	private bool wallJump = false;

	public bool keyIN = true;
	
	public float moveForce = 365f;			
	public float maxSpeed = 5f;			
	public float jumpForce = 650f;
	public float wallJumpForce = 500f;
	public float wallJpDelaytime =0.3f;
	
	//private Transform groundCheck;
	public bool Grounded = false;
	//private Transform wallJumpCheck;
	public bool wallJumped = false;
	
	//Player Health//主角武器系統
	public int actionID = 0;
	public float AtkCoolTime = 0.9f;
	public float lastAtkTime;// The time at which the player was last hit.

	public bool canAtk1 = true;
	public bool canAtk3 = false;
	public float ComboTime = 0.2f;

	public AudioClip[] sound;
	AudioSource NewAudio;

	
	private Animator anim;
	AnimatorStateInfo stateInfo;

	
	void Awake()
	{
		// Setting up references.
		//groundCheck = transform.Find("groundCheck");
		//wallJumpCheck = transform.Find("wallJumpCheck");
		anim = GetComponent<Animator>();
		NewAudio = GetComponent<AudioSource> ();
	}

	public void AtkButton(){
		AtkAction ();
	}

	public void JumpButton(){
		if (Grounded) {
			jump = true;
		} else if (wallJumped) {
			keyIN = false;
			wallJump = true;
		}
	}

	public void RightArrowButton(){
		RButtonDown = true;
		LButtonDown = false;
	}

	public void RArrowButtonOff(){
		RButtonDown = false;
	}

	public void LeftArrowButton(){
		LButtonDown = true;
		RButtonDown = false;
	}

	public void LArrowButtonOff(){
		LButtonDown = false;
	}

	void Update()
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		//Grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		//wallJumped = Physics2D.Linecast(transform.position, wallJumpCheck.position, 1 << LayerMask.NameToLayer("Wall"));  
		// If the jump button is pressed and the player is grounded then the player should jump.
		//無敵時間減免
		
		if (Input.GetButtonDown ("Jump") && Grounded) {
			jump = true;
		}
		if (Input.GetButtonDown ("Jump") && wallJumped) {
			keyIN = false;
			wallJump = true;
		}
		anim.SetBool ("Grounded", Grounded);
		anim.SetBool ("wallJumped", wallJumped);
		
		// 5 - Shooting
		bool ATK = Input.GetKeyDown(KeyCode.Z);
		//shoot |= Input.GetButtonDown("Fire2");
		// Careful: For Mac users, ctrl + arrow is a bad idea
		
		if (ATK) {
			AtkAction ();
		}

	}

	void AtkAction() {
		if (Time.time > lastAtkTime + AtkCoolTime && canAtk1) {
			//StartCoroutine (Atk1Audio(0.00f));
			anim.SetInteger ("actionID", 0);
			anim.SetTrigger ("actionT");
			lastAtkTime = Time.time;
			ComboTime = 0.5f;
		} else if (ComboTime > 0 && anim.GetInteger ("actionID") == 0) {
			anim.SetInteger ("actionID", 1);
			StartCoroutine (Atk2Play (0.1f));
			lastAtkTime = Time.time;
			ComboTime += 1f;

		} else if (ComboTime > 0 && anim.GetInteger ("actionID") == 1 && canAtk3) {
			anim.SetBool ("actionT2", true);
			//anim.SetInteger("actionID",2);
			StartCoroutine(animAtkPlay (0.2f));

		}
	}

	//攻擊動畫撥放延遲
	IEnumerator Atk2Play(float waitTime){
		anim.SetTrigger ("actionT1");
		yield return new WaitForSeconds (waitTime);
		//NewAudio.PlayOneShot(sound[2],0.5f);
		anim.SetInteger ("actionID", 1);
		lastAtkTime = Time.time;
		yield break;
	}

	//攻擊動畫撥放延遲
	IEnumerator animAtkPlay(float waitTime){
		yield return new WaitForSeconds (waitTime);
		anim.SetBool ("actionT2", false);
		//StartCoroutine (Atk1Audio(0.00f));
		//startAtk3Audio ();
	}

	void MoveAction(float h) {
		showH = h;
		if (h * GetComponent<Rigidbody2D> ().velocity.x < maxSpeed) {
			GetComponent<Rigidbody2D> ().AddForce (Vector2.right * h * moveForce);
		}
		if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) > maxSpeed) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	void FixedUpdate ()	{
		if (ComboTime > 0) {
			ComboTime -= Time.deltaTime;
		} else {
			anim.SetInteger ("actionID", 0);
		}
		float h = Input.GetAxis ("Horizontal");
		// Cache the horizontal input.
		if (isMobile == false) {
			if (keyIN == true) {
				// The Speed animator parameter is set to the absolute value of the horizontal input.
				if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow)) {
					anim.SetFloat ("Speed", 1);
				}
				if (Input.GetKey (KeyCode.LeftArrow) == false && Input.GetKey (KeyCode.RightArrow) == false) {
					anim.SetFloat ("Speed", 0);
				}
				MoveAction (h);
			}
		} else {
			if (keyIN == true) {
				mobileMove ();
				MoveAction (ButtonH);
			}
		}

		
		if ((ButtonH > 0 || h > 0) && !facingRight) {
			// ... flip the player.翻轉人物(向右)讓facingRight = true
			Flip();
			face = 1;
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		//如果人物向左移動而人物面向右邊
		else if ((ButtonH < 0 || h < 0) && facingRight) {
			// ... flip the player.翻轉人物使其向左 讓facingRight = fales
			Flip();
			face = -1;
		}

		// If the player should jump...
		//一般跳躍
		if(jump){
			//NewAudio.is
			NewAudio.PlayOneShot(sound[0],0.5f);
			anim.SetTrigger("Jump");
			GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce));
			jump = false;
		}
		//蹬牆跳
		if(wallJump){
			if(Input.GetAxis ("Horizontal") != 0 || ButtonH != 0){
				//anim.SetBool ("wallJump", true);
				NewAudio.PlayOneShot(sound[0],0.5f);
				anim.SetTrigger("Jump");
				GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				Flip();
				StartCoroutine(wallJumpDelayTime(wallJpDelaytime));
				if (Input.GetAxis ("Horizontal") > 0 || ButtonH > 0){
					GetComponent<Rigidbody2D>().AddForce(new Vector2(-200f, jumpForce));
				}else if(Input.GetAxis ("Horizontal") < 0 || ButtonH < 0){
					GetComponent<Rigidbody2D>().AddForce(new Vector2(200f, jumpForce));
				}
			}
			wallJump = false;
		}
	}

	void mobileMove(){
		if (RButtonDown) {
			anim.SetFloat ("Speed", 1);
			if (ButtonH < 1f) {
				ButtonH += 0.03f;
			} else {
				ButtonH = 1f;
			}
		} else if (LButtonDown) {
			anim.SetFloat ("Speed", 1);
			if (ButtonH > -1f) {
				ButtonH -= 0.03f;
			} else {
				ButtonH = -1f;
			}
		} else if (RButtonDown == false) {
			if (ButtonH > 0) {
				ButtonH = 0;
			}
		}
		if (LButtonDown == false) {
			if (ButtonH < 0) {
				ButtonH = 0;
			}
		}

		if (RButtonDown == false && LButtonDown == false) {
			anim.SetFloat ("Speed", 0);
		}
	}
	
	//翻轉人物
	void Flip()	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	
	//蹬牆跳控制延遲
	IEnumerator wallJumpDelayTime(float waitTime){
		yield return new WaitForSeconds (waitTime);
		keyIN = true;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			Grounded = true;
		} else if (Grounded == false){
			if (col.gameObject.tag == "Wall" && (Input.GetAxis ("Horizontal") != 0 || ButtonH != 0)) {
				wallJumped = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			Grounded = true;
			wallJumped = false;
		} else if (Grounded == false){
			if (col.gameObject.tag == "Wall" && (Input.GetAxis ("Horizontal") != 0 || ButtonH != 0)) {
				wallJumped = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			Grounded = false;
		}else if (col.gameObject.tag == "Wall") {
			wallJumped = false;
		}
	}


	//攻擊音效延遲
	IEnumerator Atk1Audio(float waitTime){
		Random.seed = System.Guid.NewGuid ().GetHashCode ();
		NewAudio.PlayOneShot(sound[Random.Range(2,3)],0.5f);
		yield break;
	}

	/*
	public void startAtk2Audio(){
		StartCoroutine (Atk2Audio (0.0f));
	}

	public void startAtk3Audio(){
		StartCoroutine (Atk3Audio (0.2f));
	}
	//攻擊音效延遲
	IEnumerator Atk2Audio(float waitTime){
		yield return new WaitForSeconds (waitTime);
		NewAudio.PlayOneShot(sound[2],0.5f);
	}

	//攻擊音效延遲
	IEnumerator Atk3Audio(float waitTime){
		yield return new WaitForSeconds (waitTime);
		NewAudio.PlayOneShot(sound[3],0.8f);
	}*/


}