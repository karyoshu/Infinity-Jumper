using UnityEngine;
using System.Collections;

public class wallSetup : MonoBehaviour {

	public BoxCollider2D leftWallCollider;
	public BoxCollider2D rightWallCollider;
	Camera mainCam;
	float spriteHeight;

	void Awake()
	{
		mainCam = Camera.main;
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();		
		spriteHeight = sRenderer.sprite.bounds.size.y;				//getting sprite's height
	}
	// Use this for initialization
	void Start () {
		Transform leftwallTransform = transform.FindChild ("leftWall");
		Transform rightwallTransform = transform.FindChild ("rightWall");

		Vector2 leftWallCenterPos = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);
		Vector2 rightWallCenterPos = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f, 0f);

		leftwallTransform.position = new Vector3 (leftWallCenterPos.x, leftwallTransform.position.y, leftwallTransform.position.z);
		rightwallTransform.position = new Vector3 (rightWallCenterPos.x, rightwallTransform.position.y, rightwallTransform.position.z);

		leftWallCollider.size = new Vector2(0.1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height*2f, 0f)).y);
		rightWallCollider.size = new Vector2(0.1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height*2f, 0f)).y);

	}

}
