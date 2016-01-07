using UnityEngine;
//using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public GameObject[] FadeIn;
	public AudioClip[] sound;
	private bool canStartLoad = false;
	AudioSource NewAudio;

	// Use this for initialization
	void Start () {
		Input.multiTouchEnabled = true;
		NewAudio = GetComponent<AudioSource> ();
		StartCoroutine (BackGroundfadeIn ());
		//AndroidInput.
	}
	
	// Update is called once per frame
	void Update () {
		if (canStartLoad) {
			if (Input.anyKeyDown) {
				canStartLoad = false;
				StartCoroutine (waitToload ());
			}
		}
	}

	IEnumerator waitToload()
	{
		//異步讀取場景。
		//Globe.loadName 就是A場景中需要讀取的C場景名稱。
		NewAudio.PlayOneShot(sound[0],1f);
		yield return new WaitForSeconds (0.1f);
		//讀取完畢後返回， 系統會自動進入C場景
		Application.LoadLevelAsync ("loading");
		//SceneManager.LoadSceneAsync ("loading");
	}

	//淡入
	IEnumerator BackGroundfadeIn(){
		float i = 0;
		while(i < 1){
			FadeIn[0].GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i += 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		StartCoroutine (BackTreefadeIn ());
	}

	//淡入
	IEnumerator BackTreefadeIn(){
		float i = 0;
		while(i < 1){
			FadeIn[1].GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i += 0.03f;
			yield return new WaitForSeconds (0.01f);
		}
		StartCoroutine (TapToStartfadeIn ());

	}

	//淡入
	IEnumerator TapToStartfadeIn(){
		float i = 0;
		while(i < 1){
			FadeIn[2].GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i += 0.5f;
			yield return new WaitForSeconds (0.01f);
		}
		if (FadeIn [2].GetComponent<FadeOutIn> () != null) {
			FadeIn [2].GetComponent<FadeOutIn> ().CanStartFade = true;
		}
		canStartLoad = true;
	}

}
