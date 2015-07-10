using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
	public Transform bckgrnd;
	private Camera cam;
	private Transform player;		// Reference to the player's transform.
	float offsetX;
	void Awake ()
	{
		// Setting up the reference.
		cam = Camera.main;
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Start(){
		SpriteRenderer bckgrndSprite = bckgrnd.GetComponent<SpriteRenderer> ();
		float bckgrndWidth = bckgrndSprite.sprite.bounds.size.x;
		float bckgrndHeight = bckgrndSprite.sprite.bounds.size.y;
		float camExtend = cam.orthographicSize;
		float res = cam.aspect ;
		if(1.2<res&&res<1.3)
			offsetX = 3.4f;
		else if(1.3<res&&res<1.4)
			offsetX = 4.2f;
		else if(1.4<res&&res<1.55)
			offsetX = 5.8f;
		else if(1.55<res&&res<1.65)
			offsetX = 6.7f;
		else if(1.7<res&&res<1.8)
			offsetX = 8.5f;
		//float camVerticalExtend = cam.orthographicSize;
		maxXAndY = new Vector2 (bckgrndWidth / 2 - camExtend - offsetX , bckgrndHeight / 2 - camExtend);
		minXAndY = new Vector2 (camExtend - bckgrndWidth / 2 + offsetX , camExtend - bckgrndHeight / 2);
	}

	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}


	void FixedUpdate ()
	{
		TrackPlayer();
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
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
