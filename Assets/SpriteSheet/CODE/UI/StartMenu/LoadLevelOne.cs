using UnityEngine;
//using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevelOne : MonoBehaviour {

	//private float fps = 10.0f;
	//private float nowTime;
	//一組動畫的貼圖，在編輯器中賦值。
	//public Texture2D[] animations;
	//private int nowFram;
	//異步對像
	AsyncOperation async;
	//讀取場景的進度，它的取值範圍在0 - 1 之間。
	int progress = 0;

	public bool loadingStart = false;

	public GameObject ProgressBar;
	public GameObject PlayerOnly;
	public GameObject X;
	public float distance;
	public GUISkin FontStytle;

	private Vector3 ProgressScale;

	// Use this for initialization
	void Start () {/*
		if (PlayerOnly = null) {
			PlayerOnly = GameObject.Find ("PlayerOnlySprite");
		}
		if (X = null) {
			X = GameObject.Find ("x");
		}*/
		distance = Mathf.Abs((PlayerOnly.transform.position.x - X.transform.position.x)/100);
		ProgressScale = ProgressBar.transform.localScale;
		StartCoroutine(loadScene());
	}

	//注意這裡返回值一定是 IEnumerator
	IEnumerator loadScene()
	{
		yield return new WaitForSeconds (0.5f);
		loadingStart = true;
		//異步讀取場景。
		//Globe.loadName 就是A場景中需要讀取的C場景名稱。
		async = Application.LoadLevelAsync("levelOne");
		//async = SceneManager.LoadSceneAsync("levelOne");
		//讀取完畢後返回， 系統會自動進入C場景
		yield return async;
	}

	void OnGUI(){
		//因為在異部讀取場景，
		//所以這裡我們可以刷新UI
		GUI.skin = FontStytle;
		DrawAnimation(/*animations*/);
	}


	// Update is called once per frame
	void Update () {
		//在這裡計算讀取的進度，
		//progress 的取值範圍在 0.1 ~ 1 之間，但它不會等於1
		//為了計算百分比，直接乘以100即可
		if (loadingStart) {
			progress = (int)(async.progress * 100);
			UpdateProgressBar ();
			//有了讀取進度的數值，就可以自行製作進度條
			Debug.Log ("Loading" + progress);
		}
	}

	//更新生命條及頭像
	void UpdateProgressBar() {
		ProgressBar.transform.localScale = new Vector2(ProgressScale.x * progress , 0.5f);
		PlayerOnly.transform.position = new Vector3 ((-6.8f + (distance * progress)), PlayerOnly.transform.position.y, PlayerOnly.transform.position.z);
	}

	void DrawAnimation(/*Texture2D[] tex*/){
		//在這裡顯示讀取的進度
		GUI.Label (new Rect (Screen.width / 10 , Screen.height / 1.2f , 500, 200), "Loading...  " + progress + "%");

		/*
		nowTime += Time.deltaTime;
		if (nowTime >= 1.0 / fps) {
			nowFram++;
			nowTime = 0;
			if (nowFram >= tex.Length) {
				nowFram = 0;
			}
		}
		GUI.DrawTexture (new Rect (100, 100, 40, 60), tex [nowFram]);*/
	}
}
