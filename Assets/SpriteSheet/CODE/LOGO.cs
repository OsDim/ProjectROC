using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class LOGO : MonoBehaviour {

	public GameObject Logo;
	public bool StartFadeOut = true;

	// Use this for initialization
	void Start () {
		Logo = GameObject.Find ("LOGO");
	}

	// Update is called once per frame
	void Update () {
		if (Logo == null) {
			Logo = GameObject.Find ("LOGO");
		} else if (StartFadeOut) {
			StartCoroutine (fadeIn ());
		}
	}

	//淡入
	IEnumerator fadeIn(){
		StartFadeOut = false;
		float i = 0;
		while(i < 1){
			Logo.GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i += 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		Logo.GetComponent<SpriteRenderer>().color = new Color (1,1,1,1);
		yield return new WaitForSeconds (1.5f);
		StartCoroutine (fadeOut ());
	}

	//淡出
	IEnumerator fadeOut(){
		StartFadeOut = false;
		float i = 1;
		while(i > 0){
			Logo.GetComponent<SpriteRenderer>().color = new Color (1,1,1,i);
			i -= 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (0.5f);
		Application.LoadLevel ("StartMenu");
		//SceneManager.LoadScene ("StartMenu");
	}
}
