using UnityEngine;
using System.Collections;

public class Area1 : MonoBehaviour {

	public bool active = false;

	//先宣告會被啟動的物件的內涵程式。
	private EnemyScript rice1;
	private EnemyScript rice2;

	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		rice1 = GameObject.Find ("Area1/rice-simple").GetComponent<EnemyScript> ();
		//rice2 = GameObject.Find ("rice-simple1").GetComponent<EnemyScript> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if (active != true) {
				active = true;
				rice1.Spawn();
				//rice2.Spawn();
			}
		}
	}
}
