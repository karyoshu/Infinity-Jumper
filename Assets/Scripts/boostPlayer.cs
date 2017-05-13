using UnityEngine;
using System.Collections;

public class boostPlayer : MonoBehaviour {

	GameObject player;
	PlayerControl playerControl;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");		//refrence to player gamebject
        playerControl = player.GetComponent<PlayerControl>();		//refrence to playerControl script attached to player gameobject
	}
	void OnTriggerEnter2D()
	{
		if(player.GetComponent<Rigidbody2D>().velocity.y < 0)		//if player is going down and comes in contact than boost player
			playerControl.SendMessage ("Boost");
	}
}
