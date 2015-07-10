using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public Vector3 offset;			// The offset at which the Health Bar follows the player.
	
	private Transform player;		// Reference to the player.
	Vector3 newposition;			//Vector to store new position.

	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update ()
	{
		newposition = new Vector3 (offset.x , offset.y + player.position.y , offset.z);		//updating new position to the player's y position plus offset.
		//newposition = 
		transform.position = newposition;	//updating camera's position to new position
	}
}
