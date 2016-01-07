using UnityEngine;
using System.Collections;

public class cameraFollowPlayer : MonoBehaviour 
{
	public Transform target;
	public GameObject stopWall;
	public GameObject CamTop;
	public GameObject CamBot;

	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.

	public Vector2 showTarget;

	/*
	public Vector3 want;
	public float radius;
	public float Smooth;
	public float distance = 5.0f;
	public float height = 3.0f;
	public float width = 3.0f;
	public float damping = 5.0f;//減震
	*/
	public GameObject HpBar;

	private Transform player;		// Reference to the player's transform. 

	void Awake ()
	{
		// Setting up the reference.
		if (target != null) {
			player = target.transform;
		}
	}

	void Start () {
		if (target == null) {
			this.enabled = false;
		}
		if (stopWall != null) {
			stopWall.SetActive (false);
		}
		if (CamTop != null) {
			CamTop.SetActive (false);
		}
		if (CamBot != null) {
			CamBot.SetActive (false);
		}
		StartCoroutine (wait ());
		StartCoroutine (zoomOut ());
		if (HpBar != null) {
			HpBar.transform.localPosition = new Vector3 (-1.75f, 1.11f, 7f);
			HpBar.transform.localScale = new Vector3 (0.12f, 0.12f, 1f);
			/*
			HpBar.transform.localPosition = new Vector3 (-2.4f, 1.5f, 7f);
			HpBar.transform.localScale = new Vector3 (0.155f, 0.155f, 1f);
			*/
		}
	}

	void FixedUpdate ()
	{

		TrackPlayer ();
		/*---------------------------------------------------------------------------------------------------
		Vector3 wantedPosition = target.TransformPoint (width, height, -distance);
		want = wantedPosition;
		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
		-----------------------------------------------------------------------------------------------------
		//if (Mathf.Abs(transform.position.x - wantedPosition.x))
		//Vector3 cameraPos = Vector3.Lerp (transform.position, actor.transform.position, Time.deltaTime);
		//transform.position = new Vector3 (cameraPos.x, cameraPos.y, -10);
		//Vector3 cameraPos = Vector3.Lerp (startMarker.position, endMarker.position, Time.deltaTime);
		//transform.position = new Vector3 (endMarker.position.x, startMarker.position.y, -2f);
		*/
	}

	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs((transform.position.y + 1.3f) - player.position.y) > yMargin;
	}
		
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y  + 0.2f, player.position.y, ySmooth * Time.deltaTime);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);//Mathf.Clamp 限制變數在最小值與最大值之間，若>max則回傳max，小於min則回傳min，中間則回傳原值
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		showTarget = new Vector2(targetX, targetY);
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}

	IEnumerator zoomOut () {
		float i = 0;
		while (GetComponent<Camera> ().orthographicSize < 5.8f) {
			i += 0.01f;
			GetComponent<Camera> ().orthographicSize += i;
			yield return new WaitForSeconds (0.05f);
		}
		GetComponent<Camera> ().orthographicSize = 6f;
		yield break;
	}

	IEnumerator wait () {
		yield return new WaitForSeconds (0.1f);
		yield break;
	}
	
}
