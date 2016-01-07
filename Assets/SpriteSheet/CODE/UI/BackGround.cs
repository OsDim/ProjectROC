using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

	public float Time;
	public int caseSwitch = 1;
	public bool canMove = false;
	private Vector3 newPos;
	private Vector3 topPos = new Vector3 (0, 5, 10);
	private Vector3 botPos = new Vector3 (0, 1, 10);

	// Use this for initialization
	void Start () {
		StartCoroutine (upToTop()/*upToTop()*/);	

	}

	// Update is called once per frame
	void Update () {
		if (canMove) {
			canMove = false;
			switch (caseSwitch)
			{
			case 1:
				StartCoroutine (upToTop());	
				break;
			case 2:
				StartCoroutine (downToBot());	
				break;
			default:
				break;
			}

		}

	}

	//StartCoroutine (getHurt (0.8f));
	IEnumerator upToTop() {
		while (transform.position.y < 4.5){
			Vector3 newPos = Vector3.Lerp (transform.position, topPos, 0.05f);
			transform.position = new Vector3 (transform.position.x, newPos.y, transform.position.z);
			yield return new WaitForSeconds (0.1f);
		}
		caseSwitch = 2;
		StartCoroutine (delay(Random.Range (4f, 10f)));	
	}

	IEnumerator downToBot() {
		while (transform.position.y > 1.5){
			Vector3 newPos = Vector3.Lerp (transform.position, botPos, 0.05f);
			transform.position = new Vector3 (transform.position.x, newPos.y, transform.position.z);
			yield return new WaitForSeconds (0.1f);
		}
		caseSwitch = 1;
		StartCoroutine (delay(Random.Range (4f, 10f)));	
	}

	IEnumerator delay(float delaytime) {
		yield return new WaitForSeconds (delaytime);
		canMove = true;
	}
}
