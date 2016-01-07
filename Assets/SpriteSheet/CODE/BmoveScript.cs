using UnityEngine;
using System.Collections;

public class BmoveScript : MonoBehaviour {
	public Vector2 speed = new Vector2(10, 10);
	public Vector2 direction = new Vector2(1, 0);
	
	private Vector2 movement;
	private Animator anim;
	// Use this for initialization

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start () {
		//movement = new Vector2(speed.x * direction.x,speed.y * direction.y);
	}
	
	// Update is called once per frame
	void Update () {
		movement = new Vector2(speed.x * direction.x,speed.y * direction.y);
	}

	void FixedUpdate ()
	{
		if (anim.GetBool ("Explosion") != true) {
			GetComponent<Rigidbody2D> ().velocity = movement;
		}
	}
}


