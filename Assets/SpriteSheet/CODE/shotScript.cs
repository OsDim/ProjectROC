using UnityEngine;
using System.Collections;
	
public class shotScript : MonoBehaviour {
	public int damage = 1;
	public bool isEnemyShot = false;
	public bool Boom = false;
	private Animator anim;

	// Use this for initialization
		
	void Awake(){
		anim = GetComponent<Animator>();
	}

	void Start () {
	
	}
		
	// Update is called once per frame
	void Update () {
		if (Boom) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ground" || col.gameObject.tag == "Enemy") {
			anim.SetBool("Explosion",true);
			GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		}
	}

	void FixedUpdate(){
	//limited time to live to avoid any leak
	Destroy (gameObject, 3); //sec
	}

}
