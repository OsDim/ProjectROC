using UnityEngine;
using System.Collections;

public class EnemyHealthScript : MonoBehaviour 

{
	public float hp = 1;
	public float realHp;
	public bool isAlive = true;

	public float hurtForce = 50;
	public float DamageCoolTime = 0.5f;
	public float lastHitTime;					// The time at which the player was last hit.

	public Vector3 hurtV;

	public bool isEnemy = true;
	public bool isRiceSimple = false;//case1
	public bool isRiceFast = false;//case2
	public bool isRabbit = false;//case3
	public bool isTaiwanBear = false;//case4
	public bool isCow = false;//case10
	public bool Die = false;//case10
	private int caseSwitch = 0;
	
	public GameObject thisEnemy;
	public GameObject player;

	public Transform spawnPos;
	public GameObject point;

	public Transform playerPos;

	public float DmgCount;
	public GameObject dmgPoint;
	public GameObject HitEff;

	public bool direction = false;

	public AudioClip[] sound;
	AudioSource NewAudio;

	private Animator anim;

	void Awake(){
		player = GameObject.Find("Player");
		anim = GetComponent<Animator>();
		NewAudio = GetComponent<AudioSource> ();

	}

	void Start(){
		if (isRiceSimple) {
			caseSwitch = 1;
			realHp = hp * Random.Range (39, 43);
		} else if (isRiceFast) {
			caseSwitch = 2;
			realHp = hp * Random.Range (22, 26);
		} else if (isRabbit) {
			caseSwitch = 3;
			realHp = hp * Random.Range (21, 24);
		} else if (isTaiwanBear) {
			caseSwitch = 4;
			realHp = hp * Random.Range (69, 96);//TaiwanBear
		} else if (isCow) {
			caseSwitch = 10;
			realHp = hp * Random.Range (69, 89);//Cow
		}
		if (Die) {
			StartCoroutine (Dead ());
		}
	}

	public void Damage(float damageCount)//受到攻擊呼叫Damage 傳入原始傷害值
	{
		if (Time.time > lastHitTime + DamageCoolTime) {
			StartCoroutine (TakeDamage (playerPos));
			lastHitTime = Time.time;
			DmgSwitch (damageCount);
			DmgP ();
			realHp -= DmgCount;
			//-------------------------------------------------------

			//判斷受到攻擊的方向
			if (transform.position.x - player.transform.position.x > 0) {
				direction = true;
			} else {
				direction = false;
			}

			//死亡判斷====================================================
			if (realHp <= 0) {
				if (isAlive){
					isAlive = false;
				// Dead!
				//Destroy (gameObject);
				StartCoroutine(Dead());
				}
			}
			//====================================================
		}
	}

	void DmgSwitch(float DmgC){
		//不同怪物受到不同程度的傷害
		switch (caseSwitch) {
		case 1://riceSimple
			DmgC *= Random.Range (9, 19);//中間值14 19*2=38+1=39=40	(約三到四下)	(給予傷害較低、生命較高)
			break;
		case 2://riceFast
			DmgC *= Random.Range (21, 39);//中間值23 約26~28			(約一到二下)	(給予傷害較高、生命較低)
			break;
		case 3://rabbit
			DmgC *= Random.Range (20, 26);//中間值21	
			break;
		case 4://TaiwanBear
			DmgC *= Random.Range (16, 29);//尚未設定
			break;
		case 10://Cow
			DmgC *= Random.Range (21, 39);//尚未設定
			break;
		default:
			break;
		}
		DmgCount = DmgC;
	}

	void DmgP(){
		//彈出該次傷害的數字-產生一個DmgPoint
		GameObject dmg;
		Vector2 spawnPosDmg;
		spawnPosDmg = new Vector2 (spawnPos.transform.position.x, spawnPos.transform.position.y + 1f);
		Quaternion rot = new Quaternion (0, 0, 0, 0);
		dmg = Instantiate (dmgPoint, spawnPosDmg, rot)as GameObject;
		dmg.transform.parent = this.gameObject.transform;
		Random.seed = System.Guid.NewGuid ().GetHashCode ();
		NewAudio.PlayOneShot(sound[Random.Range (0, 2)],0.8f);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// Is this a shot?
		shotScript shot = col.gameObject.GetComponent<shotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage);
				
				// Destroy the shot
				//Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}

	IEnumerator TakeDamage (Transform enemy) {
		// Make sure the player can't jump.
			//---------------走路飯的-----------------
			if (thisEnemy.GetComponent<moveScript> () != null) {
				Vector3 hurtVector = transform.position - player.transform.position;// + Vector3.up;// * 5f;
				//hurtV = hurtVector;
				if (hurtVector.x < 0) {      
					thisEnemy.GetComponent<moveScript> ().hurtVector = -1;
				} else {
					thisEnemy.GetComponent<moveScript> ().hurtVector = 1;
				}
				thisEnemy.GetComponent<moveScript> ().stopMove = true;
			}else{}
			//---------------走路飯的-----------------
			//---------------兔子的-------------------
			if (this.gameObject.GetComponent<rabbitMove> () != null) {

				Vector3 hurtVector = transform.position - player.transform.position;// + Vector3.up;// * 5f;
				//hurtV = hurtVector;
				if (hurtVector.x > 0) {      
					
					this.gameObject.GetComponent<rabbitMove> ().TakeHurt (1);
				} else {
					this.gameObject.GetComponent<rabbitMove> ().TakeHurt (-1);
				}
			}else{}
			//---------------兔子的-------------------

		if (isCow) {
			StartCoroutine (HitColor ());
		} else {
			StartCoroutine (getHurt (DamageCoolTime));
		}
		//BossCow------------------------------------------------------------
		yield break;
		// Play a random clip of the player getting hurt.
		/*播放被攻擊音效
		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
		*/
	}

	IEnumerator Dead () {
		this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, GetComponent<Rigidbody2D> ().velocity.y);
		GameObject go;
		Vector2 spawnPos2D;
		spawnPos2D = new Vector2 (spawnPos.transform.position.x, spawnPos.transform.position.y + 1);
		Quaternion rot = new Quaternion (0, 0, 0, 0);
		go = Instantiate (point, spawnPos2D, rot)as GameObject;
		if (isCow != true) {
			this.GetComponent<BoxCollider2D> ().isTrigger = true;
			this.gameObject.GetComponent<Rigidbody2D> ().freezeRotation = false;
			this.GetComponent<Animator> ().enabled = false;
			if (this.gameObject.GetComponent<moveScript> () != null) {
				this.gameObject.GetComponent<moveScript> ().enabled = false;
			} else if (this.gameObject.GetComponent<rabbitMove> () != null) {
				this.gameObject.GetComponent<rabbitMove> ().enabled = false;
			} else {
			}
			if (direction) {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (50, 80), Random.Range (300, 400)));
			} else {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-Random.Range (50, 80), Random.Range (300, 400)));
			}
			int i = 0;
			while (true) {
				i++;
				yield return new WaitForSeconds (0.05f);
				if (direction) {
					this.gameObject.transform.Rotate (0, 0, -i);
				} else {
					this.gameObject.transform.Rotate (0, 0, i);
				}
			}
		} else {
			anim.SetBool("RunIng",false);
			anim.SetBool("RunStart",false);
			anim.SetBool("RunHandIng",false);
			anim.SetTrigger("Dead");
			StartCoroutine (fadeOut ());
		}
		//yield break;
	}


	//淡出
	IEnumerator fadeOut(){
		float i = 1;
		while(i > 0){
			thisEnemy.GetComponent<SpriteRenderer> ().color = new Color (1,1,1,i);
			i -= 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		Destroy (thisEnemy);
	}

	IEnumerator HitColor(){
		thisEnemy.GetComponent<SpriteRenderer> ().color = Color.red;
		yield return new WaitForSeconds (0.1f);
		thisEnemy.GetComponent<SpriteRenderer> ().color = Color.white;
	}

	IEnumerator getHurt(float waitTime) {
		float t = 0;
		while (t < 0.1f) {
			GetComponent<SpriteRenderer> ().color = Color.red;
			yield return new WaitForSeconds (0.15f);
			GetComponent<SpriteRenderer> ().color = Color.white;
			yield return new WaitForSeconds (0.05f);
			t += 0.1f;
		}
		GetComponent<SpriteRenderer> ().color = Color.white;
	}
}

