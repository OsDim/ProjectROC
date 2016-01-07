using UnityEngine;
using System.Collections;

public class weaponScript : MonoBehaviour 
{
	public Transform shotPrefabR;
	public Transform shotPrefabL;
	public Transform shotPrefab;
	public Transform point;

	
	/// Cooldown in seconds between two shots
	/// 射擊冷卻
	public float shootingRate = 0.05f;
	public float speed = 20f;
	//--------------------------------
	// 2 - Cooldown
	private float shootCooldown;
	private playerC faceW;

	void Awake()
	{
		faceW = GetComponent<playerC>();
	}

	void Start()
	{
		shootCooldown = 0f;
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}

	//--------------------------------
	// 3 - Shooting from another script
	//--------------------------------

	/// Create a new projectile if possible
	public void Attack(bool isEnemy)
	{
		if (CanAttack)
		{
			StartCoroutine(atkDelay(0.05f));
			// Create a new shot
		}
	}
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
	//攻擊動畫撥放延遲
	IEnumerator atkDelay(float waitTime){
		yield return new WaitForSeconds (waitTime);
		shootCooldown = shootingRate;
		/*Rigidbody2D shotTransform = Instantiate (shotPrefab, point.position, Quaternion.identity)as Rigidbody2D;
		shotTransform.GetComponent<Rigidbody2D> ().velocity = (Vector2.right * 500);//不知道怎麼設定2DSprite初速*/
		if (faceW.facingRight){
			//var shotTransform = Instantiate(shotPrefabR) as Transform;
			Rigidbody2D shotTransform = Instantiate(shotPrefabR, point.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			// Assign position
			
			// The is enemy property
			/*shotScript shot = shotTransform.gameObject.GetComponent<shotScript>();
				if (shot != null)
				{
					shot.isEnemyShot = isEnemy;
				}
				
				// Make the weapon shot always towards it
				moveScript move = shotTransform.gameObject.GetComponent<moveScript>();
				if (move != null)
				{
					move.direction = this.transform.right; // towards in 2D space is the right of the sprite
				}*/
		}else{
			//var shotTransform = Instantiate(shotPrefabR) as Transform;
			Rigidbody2D shotTransform = Instantiate(shotPrefabL, point.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			// Assign position
			
			// The is enemy property
			/*shotScript shot = shotTransform.gameObject.GetComponent<shotScript>();
				if (shot != null)
				{
					shot.isEnemyShot = isEnemy;
				}
				
				// Make the weapon shot always towards it
				moveScript move = shotTransform.gameObject.GetComponent<moveScript>();
				if (move != null)
				{
					move.direction = this.transform.right; // towards in 2D space is the right of the sprite
				}*/
		}
	}
}

