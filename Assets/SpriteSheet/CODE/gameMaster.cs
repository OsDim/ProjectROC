using UnityEngine;
//using UnityEngine.SceneManagement;
using System.Collections;
using Image = UnityEngine.UI.Image;

public class gameMaster : MonoBehaviour {

	public Transform spawnPos;
	private GameObject playerInScene;
	public GameObject myPlayer;
	public bool yee = true;
	public float PlayerHp;

	//public GameObject reStart;
	public GameObject DeathBlackIn;
	public GameObject logoOut;
	public  bool FadeOutIn = true;
	public GameObject UiButton;

	public GameObject PauseButton;
	public Sprite PauseButton1;
	public Sprite PauseButton2;

	public GameObject MonsterCreater;

	//死亡視窗
	public float windowWidth = Screen.width;
	public float windowHight = Screen.height;

	Rect windowRect;
	int windowSwitch = 0;
	float alpha = 0;

	private bool reStartEnabled = false;
	private bool pause = false;

	//畫面基數
	private float baseW = 1280;
	private float baseH = 720;
	//畫面比例
	private Vector2 scale;

	void Awake(){
		windowWidth = Screen.width;
		windowHight = Screen.height/2;
		scale = new Vector2 (Screen.width / baseW, Screen.height / baseH);

	}

	// Use this for initialization
	void Start () {
		playerInScene = GameObject.Find("playerHealth");
		//reStart = GameObject.Find("reStart");
		DeathBlackIn = GameObject.Find("DeathBlackIn");
		logoOut = GameObject.Find("logoOut");
		windowRect = new Rect ((Screen.width - windowWidth) / 2, (Screen.height - windowHight) / 3, windowWidth, windowHight);
	}

	void GUIAlphaColor_0_To_1(){
		if (alpha < 1) {
			alpha += Time.deltaTime;
			GUI.color = new Color (1, 1, 1, alpha);
		}
	}
	public void UIPause(){
		pause = !pause;
		/*if (PauseButton.GetComponent<Image> ().sprite == PauseButton1) {
			PauseButton.GetComponent<Image> ().sprite = PauseButton2;
		} else {
			PauseButton.GetComponent<Image> ().sprite = PauseButton1;
		}*/
	}

	// Update is called once per frame
	void Update () {
		PlayerHp = myPlayer.GetComponent<playerHealthScript> ().hp;
		if (PlayerHp <= 0) {
			MonsterCreater = GameObject.Find ("RandomCreat");
			if (FadeOutIn) {
				FadeOutIn = false;
				if (MonsterCreater != null) {
					MonsterCreater.SetActive (false);
				}
				UiButton.SetActive (false);
				StartCoroutine (StartDeathFade ());
				StartCoroutine (StartlogoOutFade());
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			pause = !pause;
		}
		if (pause) {
			PauseButton.GetComponent<Image> ().sprite = PauseButton2;
			Time.timeScale = 0;
		} else {
			PauseButton.GetComponent<Image> ().sprite = PauseButton1;
			Time.timeScale = 1;
		}
	}

	void  OnGUI (){
		GUIUtility.ScaleAroundPivot (scale, Vector2.zero);
		GUI.skin.FindStyle("Button").fontSize = 60;
		if (reStartEnabled == true) {
			GUILayout.BeginHorizontal ();
			//if (GUILayout.Button ("Quit", GUILayout.Width (70), GUILayout.Height (30)) || Input.GetKeyDown ("escape")) {
				windowSwitch = 1;
				alpha = 0;
			//GUILayout.Space (30);
			//if (GUILayout.Button ("Reset", GUILayout.Width (70), GUILayout.Height (30))){
			//	SceneManager.LoadScene ("loading");
			//}
			GUILayout.EndHorizontal ();
			if (windowSwitch == 1) {
				GUIAlphaColor_0_To_1 ();
				windowRect = GUI.Window (0, windowRect, QuitWindow, "GAME OVER");
			}
			//Time.timeScale = 0;
			//GUI.Box( new Rect(Screen.width /2 - 100,Screen.height /2 - 100,250,200), "Pause Menu");
			//GUI.Label (new Rect (Screen.width / 2 - 10, Screen.height / 2 - 60, 200, 100), "尼已經死惹");
		}

		if (pause) {
			windowSwitch = 1;
			alpha = 0;
			if (windowSwitch == 1) {
				GUIAlphaColor_0_To_1 ();
				windowRect = GUI.Window (0, windowRect, PauseWindow, "Pause");
			}
			//Time.timeScale = 0;
			//GUI.Box( new Rect(Screen.width /2 - 100,Screen.height /2 - 100,250,200), "Pause Menu");
			//GUI.Label (new Rect (Screen.width / 2 - 10, Screen.height / 2 - 60, 200, 100), "尼已經死惹");
		}
	}

	void PauseWindow (int windowID){
		//GUI.Label (new Rect (100, 50, 300, 30), "Are you Sure you want to quit game?");
		if (GUI.Button (new Rect (windowWidth/20, windowHight/2, windowWidth/5, windowHight/3), "Restart")) {
			pause = !pause;
			Time.timeScale = 1;
			Application.LoadLevelAsync ("loading");
			//SceneManager.LoadScene ("loading");
		}
		if (GUI.Button (new Rect (windowWidth/3.5f, windowHight/2, windowWidth/5, windowHight/3), "Quit")) {
			Application.Quit ();
			windowSwitch = 0;
		}
		if (GUI.Button (new Rect (windowWidth/1.925f, windowHight/2, windowWidth/5, windowHight/3), "Back")) {
			pause = !pause;
			PauseButton.GetComponent<Image> ().sprite = PauseButton1;
			Time.timeScale = 1;
			windowSwitch = 0;
		}
		GUI.DragWindow ();
	}

	void QuitWindow (int windowID){
		//GUI.Label (new Rect (100, 50, 300, 30), "Are you Sure you want to quit game?");
		if (GUI.Button (new Rect (windowWidth/20, windowHight/2, windowWidth/5, windowHight/3), "Restart")) {
			Application.LoadLevel ("loading");
			//SceneManager.LoadScene ("loading");
		}
		if (GUI.Button (new Rect (windowWidth/1.925f, windowHight/2, windowWidth/5, windowHight/3), "Quit")) {
			Application.Quit ();
			windowSwitch = 0;
		}
		GUI.DragWindow ();
	}

	IEnumerator StartDeathFade(){
		float i = 0;
		while(i < 1){
			DeathBlackIn.GetComponent<SpriteRenderer> ().color = new Color (0,0,0,i);
			i += 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (0.1f);
	}

	IEnumerator StartlogoOutFade(){
		logoOut.GetComponent<SpriteRenderer> ().color = new Color (0,0,0,1);
		yield return new WaitForSeconds (0.5f);
		float i = 0;
		while(i < 1){
			logoOut.GetComponent<SpriteRenderer> ().color = new Color (i,i,i,1);
			i += 0.01f;
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (0.5f);
		reStartEnabled = true;
	}
}
