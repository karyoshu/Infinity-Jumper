using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class HorizontalTiling : MonoBehaviour {

	public int offsetX = 2;				//offset to do calculations before getting end in gameview

	public bool hasARightBuddy = false;	//used for checking if
	public bool hasALeftBuddy = false;	//instantiation of dirt is needed or not

	public bool reverseScale = false;	//used if the object is not tilable

	private float spriteWidth = 0f;		//width of element
	private Camera cam;
	private Transform myTransform;

	void Awake(){
		cam = Camera.main;
		myTransform = transform;
	}
	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasALeftBuddy || !hasARightBuddy) 	//if current sprite doesn't have a left or right sprite
		{
			//setting up camera's horizontal extend
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;	//getting sprite's right visible edge
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;	//getting sprite's left visible edge

			if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
			{
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}
			else if(cam.transform.position.x <= edgeVisiblePositionLeft - offsetX && !hasALeftBuddy)
			{
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
	}
	void MakeNewBuddy(int rightOrLeft)
	{
		//calculate the position for the top buddy of sprite, instantiate it and set current sprite parent to its parent
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;
		//if reverscale is true, reverse the sprite
		if (reverseScale) 
		{
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}
		newBuddy.parent = myTransform.parent;
		if (rightOrLeft > 0) 
		{
            newBuddy.GetComponent<HorizontalTiling>().hasALeftBuddy = true;
		}
		else 
		{
			newBuddy.GetComponent<HorizontalTiling>().hasARightBuddy = true;
		}
	}
}
