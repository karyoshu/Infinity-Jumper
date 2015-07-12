using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SpriteRenderer))]
public class TilingPlatform : MonoBehaviour {
	public int offsetY = 2;					//offset to do calculations before getting end in gameview
	public bool hasATopBuddy = false;		//check if it has a top background sprite
	public Transform[] blocks;				//blocks that can be instantiated
	float spriteHeight = 0f;				//height of the background sprite
	Camera cam;								//refrence to camera
	Transform myTransform;					//refrence to self transform
	public Transform highPoint;				//highest point of sprite
	Transform camKillPoint;					//lower point of camera, it is used to destroy background sprite after sprite is off-screen
	float screenWidthPoints;				
	SpriteRenderer sRenderer;				
	public Transform[] powerUps;			//collection of powerup prefabs
	public int powerUp;						//previous power up
	public bool newPowerUp = false;			//whether to create a new powerup with next platform
    private float currPosX = 0;				//x value of current platforms position

	void Awake()
	{
		cam = Camera.main;					//getting refrence to main camera
		//getting refrence to camera's lower point
		camKillPoint = GameObject.FindGameObjectWithTag ("KillPoint").transform;
		myTransform = transform;
	}
	
	void Start()
	{
		sRenderer = GetComponent<SpriteRenderer> ();		
		spriteHeight = sRenderer.sprite.bounds.size.y;				//getting sprite's height
		screenWidthPoints = cam.ScreenToWorldPoint (new Vector3 (0, 0, 0)).x;
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
		int randomNumber = Random.Range (1, 100);	//calculate a random no from 1 to 99
		int nextPlatform = 0;						
        currPosX = myTransform.position.x;
		//calculating the next platform's position such that it is not too far away from current platform so that player can easily reach it
        Vector3 newPosition = new Vector3(Random.Range(Mathf.Clamp(screenWidthPoints/2, currPosX - 3, currPosX), Mathf.Clamp(-screenWidthPoints/2, currPosX, currPosX +3)), myTransform.position.y + 2.2f, myTransform.position.z);
        //if current platform is a blue platform then next platform can be a different platform 
		//based upon randomness and time elapsed since the current game started
		//this is to ensure that no 2 difficult platforms come one after another
        if (sRenderer.sprite.name == "bluePlatform")
        {
            //create green platform
            if (randomNumber % 4 == 0 && Time.time - GameMaster.gm.getStartTime() > 20)
            {
                nextPlatform = 1;
                newPosition = new Vector3(0, myTransform.position.y + 2.2f, myTransform.position.z);
            }
            //create red platform
            else if (randomNumber % 6 == 0 && Time.time - GameMaster.gm.getStartTime() > 40)
            {
                nextPlatform = 2;
            }
            //create purple platform
            else if (randomNumber % 9 == 0 && Time.time - GameMaster.gm.getStartTime() > 60)
            {
                nextPlatform = 3;
            }
        }
		Transform newBuddy = Instantiate (blocks[nextPlatform], newPosition, myTransform.rotation) as Transform;

		if(newPowerUp == true)
		{
			//calculating position of new power up to be instatiated
			Vector3 newPowerUpPos = new Vector3 (newPosition.x, newPosition.y + 0.5f, newPosition.z);
			Transform powerUpTransform = Instantiate(powerUps[powerUp], newPowerUpPos, myTransform.rotation) as Transform;
			powerUpTransform.parent = myTransform.parent;
		}

		if(randomNumber%8 == 0 && !PlayerControl.Boosted)
		{
			//if player is not recently boosted next platform can have a powerup
			newBuddy.GetComponent<TilingPlatform>().newPowerUp = true;
			if(powerUp == 2)
				newBuddy.GetComponent<TilingPlatform>().powerUp = Random.Range(0,2);//if current power up is red, then next power up can be either blue or green
			else
				newBuddy.GetComponent<TilingPlatform>().powerUp = Random.Range(0,3);//next power up can be blue, green or red
		}
		else
		{
			newBuddy.GetComponent<TilingPlatform>().powerUp = powerUp;
			//tell next platform what current power up was so that no 2 red power up comes one after other
			//and tell next platform not to instantiate powerup
			newBuddy.GetComponent<TilingPlatform>().newPowerUp = false;
		}
		
		newBuddy.parent = myTransform.parent;
	}
}
