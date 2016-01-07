using UnityEngine;
using System.Collections;

public class playerHealthScript : MonoBehaviour {

	public GameObject scene;
	public GameObject player;
	public SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	public SpriteRenderer HpEffect;			// Reference to the sprite renderer of the health bar.
	public GameObject[] ComboHp;

	public Color ComboHpShow = new Color(1f,1F,1f,1f);
	public Color ComboHpHide = new Color(1f,1F,1f,0f);

	public bool CanCombo = true;
	public int ComboCount;
	public int ComboHealth;

	public float MaxHp = 100;
	public float hp = 1;
	public float oldHp;
	public bool isEnemy = true;

	public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 50f;				// The force with which the player is pushed when hurt.
	public float DamageCoolTime = 1.1f;
	public float lastHitTime;					// The time at which the player was last hit.

	public Color HpColor = new Color(0.832F,0.261F,0.164F);
	
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private Vector3 HpEffectScale;				// The local scale of the health bar initially (with full health).

	private playerC playerControl;				// Reference to the PlayerControl script.
	private Animator anim;						// Reference to the Animator on the player

	public float totalTime = 1f;

	public AudioClip[] sound;
	AudioSource NewAudio;


	void Awake () {
		// Setting up references.
		playerControl = GetComponent<playerC>();

		player = GameObject.Find("Player");
		healthBar = GameObject.Find("HP").GetComponent<SpriteRenderer>();
		HpEffect = GameObject.Find("HpEffect").GetComponent<SpriteRenderer>();
		scene = GameObject.FindWithTag ("GameScene");
		hp = MaxHp;
		oldHp = hp;
		anim = GetComponent<Animator>();
		
		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
		HpEffectScale = HpEffect.transform.localScale;
	}

	// Use this for initialization
	void Start () {
		int i = 2;
		while (i >= 0) {
			ComboHp [i].GetComponent<SpriteRenderer> ().color = ComboHpHide;
			i--;
		}
	}

	void Update ()	{
		if (CanCombo) {
			UpdateComboBar ();
		} else {
			ComboCount = 0;
		}
	}


	//受到傷害減少生命值
	public void HpDamage(float DamagePoint) {
		//anim.SetTrigger("getHit");
		hp = hp - DamagePoint;
		StartCoroutine (getHurt (0.8f));
		UpdateHealthBar();
		if (hp <= 0) {
			// Dead!
			player.GetComponent<playerC>().enabled = false;
			//GetComponentInChildren<Gun>().enabled = false;
			HpEffect.transform.localScale = healthBar.transform.localScale;
			player.GetComponent<Animator>().SetBool("die",true);
			StartCoroutine (Dead ());
			//Destroy (player);
		}
	}

	//更新生命條及頭像
	void UpdateHealthBar() {

		//控制血條顏色
		if (hp < 55) {
			if (hp < (MaxHp * 0.55)) {
				healthBar.GetComponent<SpriteRenderer> ().color = Color.Lerp (HpColor, Color.red, 1 - hp * 0.01f);
			}
		} else {
			healthBar.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		//控制血條長度
		if (hp < 0) {
			healthBar.transform.localScale = new Vector2(0.01f, 1);
		} else {
			healthBar.transform.localScale = new Vector2(healthScale.x * hp * 0.01f, 1);
		}
		StartCoroutine(HpEffectLerp(HpEffect.transform.localScale, healthBar.transform.localScale, totalTime));
	}

	//更新Combo條
	void UpdateComboBar() {
		switch (ComboCount) {
		case 1:
			ComboHp [0].GetComponent<SpriteRenderer> ().color = ComboHpShow;
			break;
		case 2:
			ComboHp [0].GetComponent<SpriteRenderer> ().color = ComboHpShow;
			ComboHp [1].GetComponent<SpriteRenderer> ().color = ComboHpShow;
			break;
		case 3:
			ComboHp [0].GetComponent<SpriteRenderer> ().color = ComboHpShow;
			ComboHp [1].GetComponent<SpriteRenderer> ().color = ComboHpShow;
			ComboHp [2].GetComponent<SpriteRenderer> ().color = ComboHpShow;
			break;
		default:
			if (ComboCount >= 3) {
				if (hp == MaxHp) {
				} else if (hp + ComboHealth > MaxHp) {
					CanCombo = false;
					hp = MaxHp;
					ComboCount = 0;
					StartCoroutine (ClearComboHp ());
				} else {
					CanCombo = false;
					hp += ComboHealth;
					ComboCount = 0;
					StartCoroutine (ClearComboHp ());
				}
			}
			break;
		}
	}

	/*
	void Update(){
	}
	*/

	public void DoScale(Vector3 start, Vector3 end, float totalTime) {
		StartCoroutine(HpEffectLerp(start, end, totalTime));
	}

	IEnumerator HpEffectLerp(Vector3 start, Vector3 end, float totalTime) {
		float t = 0;

		do {
			HpEffect.transform.localScale = Vector3.Lerp (start, end, t / totalTime);
			yield return null;
			t += Time.deltaTime;
		} while (t < totalTime);
		HpEffect.transform.localScale = end;
		yield break;
	}

	void OnTriggerEnter2D(Collider2D col) {

		// Is this a shot?
		shotScript shot = col.gameObject.GetComponent<shotScript>();
		if (col.gameObject.tag == "Enemy" || shot != null) {
			// Avoid friendly fire
			if (Time.time > lastHitTime + DamageCoolTime) {

				if(hp > 0f)	{
					EnemyScript EnemyScript = col.gameObject.GetComponent<EnemyScript>();
					if (EnemyScript != null)
					{	// Damage the enemy
						//減少生命值
						HpDamage(EnemyScript.DamagePoint);
						TakeDamage(col.transform); 
						//healthScript enemyHealth = enemy.GetComponent<healthScript>();
						//if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);
						if (col.gameObject.layer == 17){
							Destroy (shot.gameObject);
						}
						//閃爍
					}
					// 避免連續傷害
					// ... take damage and reset the lastHitTime.
					lastHitTime = Time.time; 
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D col) {

		// Is this a shot?
		shotScript shot = col.gameObject.GetComponent<shotScript>();
		if (col.gameObject.tag == "Enemy" || shot != null) {
			// Avoid friendly fire
			if (canDmg) {
				
				if(hp > 0f)	{
					EnemyScript EnemyScript = col.gameObject.GetComponent<EnemyScript>();
					if (EnemyScript != null)
					{	// Damage the enemy
						//減少生命值
						HpDamage(EnemyScript.DamagePoint);
						TakeDamage(col.transform);
						//healthScript enemyHealth = enemy.GetComponent<healthScript>();
						//if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);
						if (col.gameObject.layer == 17){
							Destroy (shot.gameObject);
						}
						//閃爍
					}
					// 避免連續傷害
					// ... take damage and reset the lastHitTime.
					lastHitTime = Time.time; 
				}
			}
		}
	}

	bool canDmg	{
		get
		{
			return Time.time > lastHitTime + DamageCoolTime;
		}
	}

	void TakeDamage (Transform enemy) {
		// Make sure the player can't jump.
		player.GetComponent<playerC>().enabled = false;
		
		// Create a vector that's from the enemy to the player with an upwards boost.
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
		
		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
		player.GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

		player.GetComponent<playerC>().enabled = true;

		// Update what the health bar looks like.
		UpdateHealthBar();
		
		// Play a random clip of the player getting hurt.
		/*播放被攻擊音效
		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
		*/
	}

	//清空Combo計
	IEnumerator ClearComboHp() {
		UpdateHealthBar ();
		int i = 2;
		while (i >= 0) {
			ComboHp [i].GetComponent<SpriteRenderer> ().color = ComboHpHide;
			i--;
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (0.7f);
		CanCombo = true;
	}

	//無敵閃爍
	IEnumerator getHurt(float waitTime) {
		float t = 0;
		while (t < waitTime) {
			player.GetComponent<SpriteRenderer>().enabled = !player.GetComponent<SpriteRenderer>().enabled;
			yield return new WaitForSeconds (0.1f);
			t += 0.1f;
		}
		player.gameObject.GetComponent<Renderer> ().enabled = true;
	}

	IEnumerator Dead () {
		//player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
		//this.GetComponent<BoxCollider2D>().isTrigger = true;
		player.GetComponent<playerC>().enabled = false;
		player.gameObject.GetComponent<Rigidbody2D> ().freezeRotation = false;
		player.GetComponent<Animator>().enabled = false;
		player.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range (-80, 80), Random.Range (400, 600)));
		int i = 0;
		while (i < 100) {
			i++;
			yield return new WaitForSeconds (0.05f);
			player.transform.Rotate (0, 0, i);
		}
	}

}
