using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerRed : MonoBehaviour {
	//script attached to red powerup that calls InvertControl method of player
	GameObject player;
	PlayerControl playerControl;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	void OnTriggerEnter2D()
	{
		playerControl.SendMessage ("InvertControl");
        GameMaster.gm.numRedBallsTaken++;
        GameMaster.gm.SendMessage("CountDown");
		Destroy (gameObject);
	}
}