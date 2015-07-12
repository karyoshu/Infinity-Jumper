using UnityEngine;
using System.Collections;

public class LeftToRight : MonoBehaviour {

	float leftBoundary;		//left boundary of screen
	float rightBoundary;	//right boundary of screen
	Transform playerRef;	//refrence to player transform
	Camera mainCam;			//refrence to main camera
	public float offset = 0.5f;
	void Awake()
	{
		playerRef = GameObject.FindGameObjectWithTag ("Player").transform;
		mainCam = Camera.main;
		leftBoundary = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - offset;
		rightBoundary = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + offset;
	}

	// Update is called once per frame
	void Update () {
		//if player goes out of left boundary move its position to right boundary
		if (playerRef.position.x < leftBoundary)
			playerRef.position = new Vector2 (rightBoundary, playerRef.position.y);
		//if player goes out of right boundary move its position to left boundary
		else if (playerRef.position.x > rightBoundary)
			playerRef.position = new Vector2 (leftBoundary, playerRef.position.y);
	}
}
