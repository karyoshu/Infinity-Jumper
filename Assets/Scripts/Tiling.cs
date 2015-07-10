using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {
	public int offsetY = 2;					//offset to do calculations before getting end in gameview
	public bool hasATopBuddy = false;		//check if it has a top background sprite

	float spriteHeight = 0f;				//height of the background sprite
	Camera cam;								//refrence to camera
	Transform myTransform;					//refrence to self transform
	public Transform highPoint;
	Transform camKillPoint;
	void Awake()
	{
		cam = Camera.main;
		camKillPoint = GameObject.FindGameObjectWithTag ("KillPoint").transform;
		myTransform = transform;
	}

	void Start()
	{
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();		
		spriteHeight = sRenderer.sprite.bounds.size.y;				//getting sprite's height
	}

	void Update()
	{
		if(!hasATopBuddy)
		{
			float camVerticalExtend = cam.orthographicSize * Screen.height/Screen.width;		//setting up camera's Vertical extend
			float edgeVisiblePositionTop = (myTransform.position.y + spriteHeight/2) - camVerticalExtend;
			float edgeVisiblePositionBottom = (myTransform.position.y - spriteHeight/2) + camVerticalExtend;
			
			if(cam.transform.position.y >= edgeVisiblePositionTop - offsetY)
			{
				MakeNewBuddy();
				hasATopBuddy = true;
			}
		}
		if(camKillPoint.position.y > highPoint.position.y)
		{
			Destroy(this.gameObject);
		}
	}

	void MakeNewBuddy()
	{
		Vector3 newPosition = new Vector3 (myTransform.position.x , myTransform.position.y + spriteHeight, myTransform.position.z);
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;
		
		newBuddy.parent = myTransform.parent;
	}
}
