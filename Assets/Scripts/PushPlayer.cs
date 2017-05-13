using UnityEngine;
using System.Collections;

public class PushPlayer : MonoBehaviour {
	GameObject player;
	PlayerControl playerControl;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");		//getting refrence to player gameObject
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();	//getting refrence to PlayerControl script attached to player
	}
	void OnTriggerEnter2D()
	{
		if(player.GetComponent<Rigidbody2D>().velocity.y < 0)	//if player is coming downwards and not going up, trigger Jump() method of PlayerControl Script
		{
			playerControl.SendMessage ("Jump");
		}
	}
}
