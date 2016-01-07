using UnityEngine;
using System.Collections;

public class HpBarPosition : MonoBehaviour {

	public Vector2 offset;
	
	private cameraScript cameraS;
	public Transform cameraPos;
	public float FinalPos;
	
	
	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		cameraS = GameObject.FindWithTag ("MainCamera").GetComponent<cameraScript> ();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		FinalPos = cameraPos.transform.position.x + offset.x;
		transform.position = new Vector2 (FinalPos, transform.position.y); 
		
	}
}
