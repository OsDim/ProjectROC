using UnityEngine;
using System.Collections;

public class DmgPoint : MonoBehaviour {

	public float ShowPoint = 0;
	public bool show = false;
	public float showT = 0.2f;

	//透明顏色------------------------------------------
	public Color Transparent = new Color(1,0,0,0);
	//透明顏色------------------------------------------
	//現在顏色
	private Color nowColer = new Color(1,0,0,1);

	private EnemyHealthScript enemyHs;
	
	// Use this for initialization
	void Start () {
		enemyHs = gameObject.GetComponentInParent<EnemyHealthScript>();
		if (enemyHs != null) {
			ShowPoint = enemyHs.DmgCount;
			GetComponent<TextMesh>().text = "-" + Mathf.RoundToInt(ShowPoint).ToString();
			if (enemyHs.direction) {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (100, 200), Random.Range (550, 750)));
			} else {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-100, 300), Random.Range (550, 750)));
			}
			transform.parent = null;
		}
		//假如傷害超過20點 → 產生暴擊效果
		if (ShowPoint >= 20) {
			showT = 0.5f;
			StartCoroutine(BtoS ());
		}
		StartCoroutine(showTime (showT));
	}
	
	// Update is called once per frame
	void Update () {

		if (show) {
			nowColer = Color.Lerp(nowColer,Transparent,0.1f);;
			GetComponent<TextMesh>().color = nowColer;
		}

	}

	//開始淡化
	IEnumerator showTime(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		show = true;
	}
	//暴擊效果 文字由大變小
	IEnumerator BtoS() {
		while (GetComponent<TextMesh>().fontSize < 40) {
			yield return new WaitForSeconds (0.01f);
			GetComponent<TextMesh>().fontSize += 5;
		}
		while (GetComponent<TextMesh>().fontSize > 25) {
			yield return new WaitForSeconds (0.03f);
			GetComponent<TextMesh>().fontSize -= 1;
		}

	}
}
