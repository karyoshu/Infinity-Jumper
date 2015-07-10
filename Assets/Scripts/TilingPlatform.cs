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
	public Transform highPoint;
	Transform camKillPoint;
	float screenWidthPoints;
	SpriteRenderer sRenderer;
	public Transform[] powerUps;
	public int powerUp;
	public bool newPowerUp = false;
    private float prevPosX = 0;

	void Awake()
	{
		cam = Camera.main;
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
		int randomNumber = Random.Range (1, 100);
		int nextPlatform = 0;
        prevPosX = myTransform.position.x;

        Vector3 newPosition = new Vector3(Random.Range(Mathf.Clamp(screenWidthPoints/2, prevPosX - 3, prevPosX), Mathf.Clamp(-screenWidthPoints/2, prevPosX, prevPosX +3)), myTransform.position.y + 2.2f, myTransform.position.z);
        
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
		
		Vector3 newPowerUpPos = new Vector3 (newPosition.x, newPosition.y + 0.5f, newPosition.z);
		Transform newBuddy = Instantiate (blocks[nextPlatform], newPosition, myTransform.rotation) as Transform;

		if(newPowerUp == true)
		{
			Transform powerUpTransform = Instantiate(powerUps[powerUp], newPowerUpPos, myTransform.rotation) as Transform;
			powerUpTransform.parent = myTransform.parent;
		}

		if(randomNumber%8 == 0 && !PlayerControl.Boosted)
		{
			newBuddy.GetComponent<TilingPlatform>().newPowerUp = true;
			if(powerUp == 2)
				newBuddy.GetComponent<TilingPlatform>().powerUp = Random.Range(0,2);
			else
				newBuddy.GetComponent<TilingPlatform>().powerUp = Random.Range(0,3);
		}
		else
		{
			newBuddy.GetComponent<TilingPlatform>().powerUp = powerUp;
			newBuddy.GetComponent<TilingPlatform>().newPowerUp = false;
		}

        newBuddy.GetComponent<TilingPlatform>().prevPosX = myTransform.position.x;
		newBuddy.parent = myTransform.parent;
	}
}
