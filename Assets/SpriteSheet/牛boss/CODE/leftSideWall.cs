using UnityEngine;
using System.Collections;

public class leftSideWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		
		print (other.name + "  leftWall");

	}

}
