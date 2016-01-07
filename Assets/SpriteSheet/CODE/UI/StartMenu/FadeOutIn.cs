using UnityEngine;
using System.Collections;

public class FadeOutIn : MonoBehaviour {

	public GameObject TapToStart;
	public bool CanStartFade = false;
	public bool StartFadeOut = true;

	// Use this for initialization
	void Start () {
		TapToStart = GameObject.Find ("TapToStart");
	}

	// Update is called once per frame
	void Update () {
		if (CanStartFade) {
			if (TapToStart == null) {
				TapToStart = GameObject.Find ("TapToStart");
			} else if (StartFadeOut) {
				StartCoroutine (fadeOut ());
			}
		}
	}

	//淡出
	IEnumerator fadeOut(){
		StartFadeOut = false;
		float i = 1;
		while(i > 0){
			TapToStart.GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i -= 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (0.3f);
		StartCoroutine (fadeIn ());
	}

	//淡入
	IEnumerator fadeIn(){
		float i = 0;
		while(i < 1){
			TapToStart.GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i += 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (Random.Range(2f, 5f));
		StartFadeOut = true;
	}

}
