using UnityEngine;
using System.Collections;

public class RandomCreat : MonoBehaviour {

	public GameObject Score;
	public int nowScore;
	public Transform spawnPos;
	public Transform BossSpawnPos;

	public GameObject GameMaster;

	public GameObject BossPrefab;
	public GameObject BossAlive;
	public GameObject BossAliveTwo;
	public bool StepOne = true;
	public bool StepTwo = true;

	public int TimeRange = 0;
	public int RandomRangeMax = 0;
	public int RandomRTop = 0;
	public int RandomRBot = 0;
	public int showK;
	private int k = 0;//怪物種類
	public int showL;
	private int L = 0;//一次生幾隻怪
	public int showI;

	public GameObject [] monster = new GameObject[6];
	private GameObject Clone;

	float timer = 10;
	//public GameObject riceSimple;
	//public GameObject riceFast;
	//public GameObject rabbit;


	// Use this for initialization
	void Start () {
		if (Score == null){
			Score = GameObject.Find ("ScoreText");
		}
		if (GameMaster == null) {
			GameMaster = GameObject.Find ("gameMaster");
		}
	}

	void RRM(){//RandomRangeMax
		if (nowScore < 1000) {
			RandomRangeMax = 1;
			TimeRange = 10;//怪物產生間隔
			L = 6;//一次要產生的怪物數量
		} else if (nowScore < 2000) {
			RandomRangeMax = 2;
			TimeRange = 8;//怪物產生間隔
			L = 5;//一次要產生的怪物數量
		} else if (nowScore < 3000) {
			RandomRangeMax = 3;
			TimeRange = 7;//怪物產生間隔
			L = 6;//一次要產生的怪物數量
		} else if (nowScore < 4000) {
			RandomRangeMax = 4;
			TimeRange = 9;//怪物產生間隔
			L = 6;//一次要產生的怪物數量
		} else if (nowScore < 5000) {//產生臺灣黑熊
			RandomRangeMax = 4;
			TimeRange = 5;//怪物產生間隔
			L = 7;//一次要產生的怪物數量
		} else if (nowScore < 7000) {//產生臺灣黑熊
			RandomRangeMax = 4;
			TimeRange = 4;//怪物產生間隔
			L = 8;//一次要產生的怪物數量
		} else if (nowScore < 9000) {//產生臺灣黑熊
			RandomRangeMax = 4;
			TimeRange = 3;//怪物產生間隔
			L = 8;//一次要產生的怪物數量
		}
		showL = L;
	}

	void MonsterChoice(){//RandomRangeMax
		Random.seed = System.Guid.NewGuid ().GetHashCode ();
		int i = Random.Range(0, RandomRangeMax);
		Random.seed = System.Guid.NewGuid ().GetHashCode ();
		int j = Random.Range (0, 99);
		showI = i;
		if (i == 0 && j < 49) {
			k = 0;//riceSimple
		} else if (i == 0 && j > 49) {
			k = 1;//riceSimpleFaceLeft
		} else if (i == 1 && j < 49) {
			k = 2;//riceFast
		} else if (i == 1 && j > 49) {
			k = 3;//riceFastFaceLeft
		} else if (i == 2) {
			k = 4;//rabbit
		} else if (i == 3 && j < 49) {
			k = 5;//TaiwanBear
		} else if (i == 3 && j > 49) {
			k = 6;//TaiwanBear
		}

		showK = k;
	}

	void BossCreat(){//產生頭目怪
		if (StepOne) {
			StepOne = false;
			Vector2 spawnPos2D;
			spawnPos2D = new Vector2 (BossSpawnPos.transform.position.x, BossSpawnPos.transform.position.y);
			//Vector3 pos = new Vector3(posx, -2.0f, -5);
			Quaternion rot = new Quaternion (0, 0, 0, 0);
			BossAlive = (GameObject)Instantiate (BossPrefab, spawnPos2D, rot);
			BossAlive.transform.position = new Vector3(BossAlive.transform.position.x, BossAlive.transform.position.y, 5f);
		} else if (StepTwo && StepOne == false && nowScore > 6900) {
			StepTwo = false;
			Vector2 spawnPos2D;
			spawnPos2D = new Vector2 (BossSpawnPos.transform.position.x, BossSpawnPos.transform.position.y);
			//Vector3 pos = new Vector3(posx, -2.0f, -5);
			Quaternion rot = new Quaternion (0, 0, 0, 0);
			BossAlive = (GameObject)Instantiate (BossPrefab, spawnPos2D, rot);
			BossAlive.transform.position = new Vector3(BossAlive.transform.position.x, BossAlive.transform.position.y, 5f);
			BossAliveTwo = (GameObject)Instantiate (BossPrefab, spawnPos2D, rot);
			BossAliveTwo.transform.position = new Vector3(BossAliveTwo.transform.position.x - 20, BossAliveTwo.transform.position.y, 5f);
		}
	}

	void MonsterCreat(){
		Vector2 spawnPos2D;
		spawnPos2D = new Vector2 (spawnPos.transform.position.x + Random.Range(-11f, 11f), spawnPos.transform.position.y + 1);
		//Vector3 pos = new Vector3(posx, -2.0f, -5);
		Quaternion rot = new Quaternion(0, 0, 0, 0);
		Clone = (GameObject)Instantiate(monster[k], spawnPos2D, rot);////(3)
		Clone.transform.position = new Vector3(Clone.transform.position.x, Clone.transform.position.y, 5f);
	}

	
	// Update is called once per frame
	void Update () {
		if (BossAlive || BossAliveTwo) {
			GameMaster.GetComponent<AudioSource> ().mute = true;
		} else {
			GameMaster.GetComponent<AudioSource> ().mute = false;
		}
		nowScore = Score.GetComponent<ScoreScript> ().score;
		if (StepOne && nowScore > 2900) {
			BossCreat ();
		}else if (StepTwo && nowScore > 6900){
			BossCreat ();
		}
		RRM ();//判斷分數區間決定出怪項目
		timer += Time.deltaTime;
		if (BossAlive == null && BossAliveTwo == null) {
			if (timer > TimeRange) {////(2)//float j = Random.Range(-1.4f, 1.4f);
				//float posx = j;
				int m = 0;
				while (m < L) {
					MonsterChoice ();//選擇要產生的怪物
					MonsterCreat ();
					m++;
					//Clone.AddComponent("FloorMove");////(4)對複製體加上腳本
				}
				timer = 0;
			}
		}
		/*
		GameObject[] AllObj = FindObjectsOfType (typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in AllObj)
		{
			if(obj.transform.position.y>20)
				Destroy(obj);////(5)
		}*/
	}
}
