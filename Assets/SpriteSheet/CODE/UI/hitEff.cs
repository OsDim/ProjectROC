using UnityEngine;
using System.Collections;

public class hitEff : MonoBehaviour {

	public int hit;
	public bool End = false;
	private Animator anim;

	// Use this for initialization
	void Start () {
		//Random.seed = System.Guid.NewGuid ().GetHashCode ();
		anim = GetComponent<Animator>();
		//hit = Random.Range (1, 4);
		//anim.SetInteger ("hitChoice", hit);
	}

	void Update () {
		if (End) {
			destroy ();
		}
	}

	void destroy () {
		Destroy (gameObject);
	}
}
