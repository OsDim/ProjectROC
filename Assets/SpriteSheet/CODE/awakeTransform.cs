using UnityEngine;
using System.Collections;

public class awakeTransform : MonoBehaviour {
	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		transform.position = new Vector3 (transform.position.x, transform.position.y, -2f);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
