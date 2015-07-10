using UnityEngine;
using System.Collections;

public class PowerBlue : MonoBehaviour {
	GameObject player;
	PlayerControl playerControl;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
	}
	void OnTriggerEnter2D()
	{
		playerControl.SendMessage ("SuperBoost");
        GameMaster.gm.greenOrBlueBallTaken = true;
		Destroy (gameObject);
	}
}