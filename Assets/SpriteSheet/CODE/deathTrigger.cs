using UnityEngine;
using System.Collections;

public class deathTrigger : MonoBehaviour {

	public GameObject player;
	public GameObject playerH;

	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		player = GameObject.FindWithTag ("Player");
		playerH = GameObject.Find ("playerHealth");
		//transform.position = new Vector3 (scene.position.x, scene.position.y + 0.32f, -2f);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindWithTag ("Player");
		}
		if (playerH == null) {
			playerH = GameObject.Find ("playerHealth");
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		print (other.name);

		if (other.tag == "Player") {
			if (playerH != null) {
				if (playerH.GetComponent<playerHealthScript> () != null)
					playerH.GetComponent<playerHealthScript> ().HpDamage (1000);
			}
		}
		if (other.tag != "Player") {
			Destroy (other.gameObject);
		}
	}
}
