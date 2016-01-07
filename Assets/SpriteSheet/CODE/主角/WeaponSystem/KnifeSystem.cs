using UnityEngine;
using System.Collections;

public class KnifeSystem : MonoBehaviour {

	public int AtkPoint = 1;
	private int DmgPoint;
	public int ShowDmgPoint;
	public Transform knifetransform;

	public GameObject PlayerHealth;
	public GameObject[] hitEff;

	private EnemyHealthScript EnemyHealth;
	private GameObject eff;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		knifetransform = transform;
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		// Is this a shot?
		shotScript shot = col.gameObject.GetComponent<shotScript>();
		EnemyHealthScript EnemyHealth = col.gameObject.GetComponent<EnemyHealthScript>();
		if (col.gameObject.tag == "Enemy") {

			// Avoid friendly fire
			if (EnemyHealth != null) {
				// Damage the enemy
				//減少生命值
				DmgPoint = (AtkPoint);
				ShowDmgPoint = DmgPoint;
				EnemyHealth.Damage(DmgPoint);
				EnemyHealth.playerPos = transform;//TakeDamage(transform);
				PlayerHealth.GetComponent<playerHealthScript>().ComboCount ++;

				Vector2 spawnPos2D;
				spawnPos2D = new Vector2 (col.transform.position.x, col.transform.position.y);
				Quaternion rot = new Quaternion (0, 0, 0, 0);
				Random.seed = System.Guid.NewGuid ().GetHashCode ();
				int i = Random.Range (0, 3);
				eff = (GameObject)Instantiate (hitEff[i], spawnPos2D, rot);
				eff.transform.position = new Vector3(eff.transform.position.x, eff.transform.position.y, 0.1f);
				//healthScript enemyHealth = enemy.GetComponent<healthScript>();
				//if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);
			}
						//閃爍
		}

					// 避免連續傷害
					// ... take damage and reset the lastHitTime.

		}

}

