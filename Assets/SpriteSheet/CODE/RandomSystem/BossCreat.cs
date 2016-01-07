using UnityEngine;
using System.Collections;

public class BossCreat : MonoBehaviour {
	
	public GameObject Score;
	public int nowScore;
	public Transform spawnPos;

	public GameObject Boss;

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
	}

	void RRM(){//RandomRangeMax
		if (nowScore < 1000) {
			RandomRangeMax = 1;
			TimeRange = 10;//怪物產生間隔
			L = 4;//一次要產生的怪物數量
		} else if (nowScore < 2000) {
			RandomRangeMax = 2;
			TimeRange = 8;//怪物產生間隔
			L = 3;//一次要產生的怪物數量
		} else if (nowScore < 3000) {
			RandomRangeMax = 2;
			TimeRange = 5;//怪物產生間隔
			L = 4;//一次要產生的怪物數量
		} else if (nowScore < 4000) {//產生兔子
			RandomRangeMax = 3;
			TimeRange = 8;//怪物產生間隔
			L = 4;//一次要產生的怪物數量
		} else if (nowScore < 5000) {//產生臺灣黑熊
			RandomRangeMax = 3;
			TimeRange = 8;//怪物產生間隔
			L = 4;//一次要產生的怪物數量
		}
		showL = L;
	}

	void MonsterChoice(){//RandomRangeMax
		int i = Random.Range(0, RandomRangeMax);
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
		} else if (i == 3) {
			k = 5;//TaiwanBear
		}
		showK = k;
	}


	// Update is called once per frame
	void Update () {

		nowScore = Score.GetComponent<ScoreScript> ().score;
		RRM ();//判斷分數區間決定出怪項目
		timer += Time.deltaTime;

		if (timer > TimeRange)////(2)
		{	//float j = Random.Range(-1.4f, 1.4f);
			//float posx = j;
			int m = 0;
			while(m < L){
				MonsterChoice();//選擇要產生的怪物
				Vector2 spawnPos2D;
				spawnPos2D = new Vector2 (spawnPos.transform.position.x + Random.Range(-11f, 11f), spawnPos.transform.position.y + 1);
				//Vector3 pos = new Vector3(posx, -2.0f, -5);
				Quaternion rot = new Quaternion(0, 0, 0, 0);
				Clone = (GameObject)Instantiate(monster[k], spawnPos2D, rot);////(3)
				m ++;
				//Clone.AddComponent("FloorMove");////(4)對複製體加上腳本
			}
			timer = 0;
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
