using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

	public bool cameraMove = false;
	public bool moveToTop = false;
	public bool moveToBot = false;
	public Transform startMarker;
	public Transform endMarker;
	public Transform cameraMovePoint;
	private Transform cameraPos;
	private checkWallScript checkWall;
	private bool difference;//用來計算cameraMovePoint 與 starMarker的差

	private float yTopRange;
	private float yBotRange;

	void Awake(){
		//player = gameObject.GetComponent<playerC>();
		checkWall = GameObject.Find ("checkWall").GetComponent<checkWallScript> ();
	}

	// Use this for initialization
	void Start () {
		GetComponent<Camera> ().orthographicSize = 4.14f;
	}
	
	// Update is called once per frame
	void Update () {
		//確保攝影機只會向右移動
		if (endMarker != null && startMarker != null) {
			if (endMarker.position.x >= startMarker.position.x) {
				difference = true;
			} else {
				difference = false;
			}
		}
		//=====================================================================================================

		//當玩家接觸到移動檢查框時移動攝影機
		if (difference && cameraMove) {
			Vector3 cameraPos = Vector3.Lerp (startMarker.position, endMarker.position, Time.deltaTime);
			transform.position = new Vector3 (endMarker.position.x, startMarker.position.y, -2f);
		}
		//=====================================================================================================
		if (moveToTop) {
			moveTop ();
		} else if (moveToBot) {
			moveBot ();
		}
	}

	/*
	void FixedUpdate ()	{
	}
	*/

	void moveTop() {
		if (transform.localPosition.y < 1f) {
			yTopRange = 1.5f;
		} else if (transform.localPosition.y < 1.5f) {
			yTopRange = 2f;
		} else if (transform.localPosition.y < 2f) {
			yTopRange = 2.5f;
		} else if (transform.localPosition.y < 4f) {
			yTopRange = 4f;
		} else {
			yTopRange = 2.5f;
		}
			
		Vector3 topPos = new Vector3 (transform.localPosition.x, yTopRange, transform.localPosition.z);
		Vector3 cameraTopPos = Vector3.Lerp	(startMarker.localPosition, topPos, Time.deltaTime);
		transform.localPosition = new Vector3 (transform.localPosition.x, cameraTopPos.y, transform.localPosition.z);//new Vector3 (MainCamera.transform.position.x, 0.5f, -2f);
	}

	void moveBot() {
		if (transform.localPosition.y < 1.5f) {
			yBotRange = 0f;
		} else if (transform.localPosition.y < 2.5f) {
			yBotRange = 1f;
		} else if (transform.localPosition.y < 3.5f) {
			yBotRange = 2f;
		} else if (transform.localPosition.y < 4.5f) {
			yBotRange = 3f;
		} else {
			yBotRange = 0f;
		}
		Vector3 botPos = new Vector3 (transform.localPosition.x, yBotRange, transform.localPosition.z);
		Vector3 cameraBotPos = Vector3.Lerp	(startMarker.localPosition, botPos, Time.deltaTime * 3f);
		transform.localPosition = new Vector3 (transform.localPosition.x, cameraBotPos.y, transform.localPosition.z);//new Vector3 (MainCamera.transform.position.x, 0.5f, -2f);
	}

}
