using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class HorizontalTiling : MonoBehaviour {

	public int offsetX = 2;				//offset to do calculations before getting end in gameview

	public bool hasARightBuddy = false;	//used for chaecking if
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
		if (!hasALeftBuddy || !hasARightBuddy) {
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;

			if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX && !hasARightBuddy)
			{
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}else if(cam.transform.position.x <= edgeVisiblePositionLeft - offsetX && !hasALeftBuddy)
			{
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
	}
	void MakeNewBuddy(int rightOrLeft)
	{
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;

		if (reverseScale) {
					newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
				}
		newBuddy.parent = myTransform.parent;
		if (rightOrLeft > 0) {
            newBuddy.GetComponent<HorizontalTiling>().hasALeftBuddy = true;
				} else {
                    newBuddy.GetComponent<HorizontalTiling>().hasARightBuddy = true;
				}
	}
}
