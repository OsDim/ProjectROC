using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
	private bool hasSpawn;
	private moveScript MoveScript;
	private rabbitMove rabbitMoveScript;
	public int DamagePoint = 1;
	public bool active = false;

	//-----------------------------------
	public Vector3 hurtV;
	public GameObject player;
	public int Direction = 0;
	//public GameObject thisEnemy;

	//private weaponScript[] weapons;
	
	void Awake()
	{
		// Retrieve the weapon only once
		//weapons = GetComponentsInChildren<weaponScript>();
		// Retrieve scripts to disable when not spawn
		MoveScript = GetComponent<moveScript>();
		rabbitMoveScript = GetComponent<rabbitMove> ();
	}

	void Start()
	{
		player = GameObject.Find("Player");
		hasSpawn = false;

		// Disable everything
		// -- collider
		this.GetComponent<BoxCollider2D>().enabled = false;
		if (this.GetComponent<Rigidbody2D> () != null) {
			this.GetComponent<Rigidbody2D> ().gravityScale = 0;
		}
		// -- Moving
		if (MoveScript != null) {
			MoveScript.enabled = false;
		}
		if (rabbitMoveScript != null) {
			rabbitMoveScript.enabled = false;
		}
		// -- Shooting
		/*foreach (weaponScript weapon in weapons)
		{
			weapon.enabled = false;
		}*/
	}


	void Update(){
		if (active) {
			active = false;
			Spawn();
		}
		Vector3 hurtVector = transform.position - player.transform.position;// + Vector3.up;// * 5f;
		hurtV = hurtVector;
		if (hurtVector.x > 0) {
			Direction = 1;
		} else {
			Direction = -1;
		}
	}


	public void Spawn(){
		hasSpawn = true;
		if (hasSpawn == true) {
			this.GetComponent<BoxCollider2D>().enabled = true;
			if (this.GetComponent<Rigidbody2D> () != null) {
				this.GetComponent<Rigidbody2D> ().gravityScale = 1;
			}
			if (MoveScript != null) {
			MoveScript.enabled = true;
			}
			if (rabbitMoveScript != null) {
				rabbitMoveScript.enabled = true;
			}
		}
	}
}
