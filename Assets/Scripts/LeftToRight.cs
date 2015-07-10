using UnityEngine;
using System.Collections;

public class LeftToRight : MonoBehaviour {

	float leftBoundary;		
	float rightBoundary;
	Transform playerRef;	//refrence to player transform
	Camera mainCam;			//refrence to main camera

	void Awake()
	{
		playerRef = GameObject.FindGameObjectWithTag ("Player").transform;
		mainCam = Camera.main;
		leftBoundary = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f;
		rightBoundary = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f;

	}

	// Update is called once per frame
	void Update () {
		if (playerRef.position.x < leftBoundary)
						playerRef.position = new Vector2 (rightBoundary, playerRef.position.y);
		else if (playerRef.position.x > rightBoundary)
				playerRef.position = new Vector2 (leftBoundary, playerRef.position.y);
	}
}
