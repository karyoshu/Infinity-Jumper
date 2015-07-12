using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {
	public int offsetY = 2;					//offset to do calculations before getting end in gameview
	public bool hasATopBuddy = false;		//check if it has a top background sprite

	float spriteHeight = 0f;				//height of the background sprite
	Camera cam;								//refrence to camera
	Transform myTransform;					//refrence to self transform
	public Transform highPoint;				//highest point of sprite
	Transform camKillPoint;					//lower point of camera, it is used to destroy background sprite after sprite is off-screen
	void Awake()
	{
		cam = Camera.main;					//getting refrence to main camera
		//getting refrence to camera's lower point
		camKillPoint = GameObject.FindGameObjectWithTag ("KillPoint").transform;
		myTransform = transform;
	}

	void Start()
	{
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();		
		spriteHeight = sRenderer.sprite.bounds.size.y;		//getting sprite's height
	}

	void Update()
	{
		if(!hasATopBuddy)	//if current sprite doesn't have a top background sprite
		{
			float camVerticalExtend = cam.orthographicSize * Screen.height/Screen.width;		//setting up camera's Vertical extend
			float edgeVisiblePositionTop = (myTransform.position.y + spriteHeight/2) - camVerticalExtend;	//getting sprite's top visible position
			
			if(cam.transform.position.y >= edgeVisiblePositionTop - offsetY)
			{
				//if camera is going above sprite's top visible position then make a top buddy
				MakeNewBuddy();
				hasATopBuddy = true;
			}
		}
		if(camKillPoint.position.y > highPoint.position.y)
		{
			//if highest point of sprite is below camera's lower point then destroy the sprite
			Destroy(this.gameObject);
		}
	}

	void MakeNewBuddy()
	{
		//calculate the position for the top budddy of sprite, instatiate it and set current sprite parent to its parent
		Vector3 newPosition = new Vector3 (myTransform.position.x , myTransform.position.y + spriteHeight, myTransform.position.z);
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;
		newBuddy.parent = myTransform.parent;
	}
}
