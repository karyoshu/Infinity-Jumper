using UnityEngine;
using System.Collections;
//script to destroy red platforms
public class DestroyPlatform : MonoBehaviour {
	GameObject player;
	PlayerControl playerControl;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");			//ref to player game object
		playerControl = player.GetComponent<PlayerControl>();			//refrence to playerControl script attached to player gameobject
	}
	void OnTriggerEnter2D()
	{
		if(player.GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			//if player is coming down and touches this platform then call playerControl script's Jump() method and destroy this platform
			playerControl.SendMessage ("Jump");
			Destroy(gameObject);
		}
	}
}
