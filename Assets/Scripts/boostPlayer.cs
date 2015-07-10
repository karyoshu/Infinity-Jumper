using UnityEngine;
using System.Collections;

public class boostPlayer : MonoBehaviour {

	GameObject player;
	PlayerControl playerControl;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
        playerControl = player.GetComponent<PlayerControl>();
	}
	void OnTriggerEnter2D()
	{
		if(player.rigidbody2D.velocity.y < 0)
			playerControl.SendMessage ("Boost");
	}
}
