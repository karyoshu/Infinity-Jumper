using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	//this script is used to move green platforms from side to side
	float movingSpeed = 3f;

	void Update()
	{
		transform.Translate (new Vector3 (movingSpeed * Time.deltaTime, 0, 0));
		if (transform.position.x > 4f ||transform.position.x < -4f)
			movingSpeed = -movingSpeed;
	}
}
