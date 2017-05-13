using UnityEngine;
using System.Collections;

public class Hider : MonoBehaviour {
	//this script is attached to purple platforms to hide them when player is far
	GameObject player;					//reference to player
	PlayerControl playerControl;		//reference to PlayerControl script attached to player
	SpriteRenderer spriteRenderer;		//reference to SpriteRenderer of platform
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	void OnTriggerEnter2D()
	{
		if(player.GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			playerControl.SendMessage ("Jump");
		}
	}

	void HideMe()
	{
        float playerX = player.transform.position.x;	//player's X position
        float playerY = player.transform.position.y;	//player's Y position
        float myX = transform.position.x;				//platform's X position
        float myY = transform.position.y;				//platform's Y position
		//calculating distance between platform and player
        float distance = Mathf.Sqrt((playerX - myX) * (playerX - myX) + (playerY - myY) * (playerY - myY));
		//sets alpha value of sprite
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1 / (distance * distance));
	}

	void Update()
	{
		HideMe ();
	}
}