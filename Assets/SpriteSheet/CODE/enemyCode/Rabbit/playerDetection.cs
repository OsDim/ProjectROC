using UnityEngine;
using System.Collections;

public class playerDetection : MonoBehaviour {

	public GameObject rabbit;
	public bool isBear = false;
	private rabbitMove rabbitScript;
	private TaiwanBearAI TaiwanBearScript;

	// Use this for initialization
	void Start () {
		if (isBear) {
			TaiwanBearScript = rabbit.GetComponent<TaiwanBearAI> ();
		} else {
			rabbitScript = rabbit.GetComponent<rabbitMove> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		//將玩家座標傳入rabbit
		if (col.gameObject.tag == "Player") {
			if (isBear) {
				TaiwanBearScript.player = col.transform;
				TaiwanBearScript.needAtk = true;
			} else {
				rabbitScript.player = col.transform;
				rabbitScript.needAtk = true;
			}
		}
	}
	
	void OnTriggerStay2D(Collider2D col) {
		//將玩家座標傳入rabbit
		if (col.gameObject.tag == "Player") {
			if (isBear) {
				TaiwanBearScript.player = col.transform;
				TaiwanBearScript.needAtk = true;
			} else {
				rabbitScript.player = col.transform;
				rabbitScript.needAtk = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		//玩家離開攻擊範圍，清空座標
		if (col.gameObject.tag == "Player") {
			if (isBear) {
				TaiwanBearScript.player = null;
				TaiwanBearScript.needAtk = false;
			} else {
				rabbitScript.player = null;
				rabbitScript.needAtk = false;
			}
		}
	}

}
