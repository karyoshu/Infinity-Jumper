using UnityEngine;
using System.Collections;

public class PushPlayer : MonoBehaviour {
	GameObject player;
	PlayerControl playerControl;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	void OnTriggerEnter2D()
	{
		if(player.rigidbody2D.velocity.y < 0)
		{
			playerControl.SendMessage ("Jump");
		}
	}
}
