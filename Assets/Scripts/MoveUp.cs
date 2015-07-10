using UnityEngine;
using System.Collections;

public class MoveUp : MonoBehaviour {

	public static float cameraSpeed = 1f;

	void Start()
	{
		//InvokeRepeating ("UpdateSpeed", 1f, 1f);
	}
	// Update is called once per frame
	void LateUpdate () {
		transform.Translate(new Vector3(0, cameraSpeed*Time.deltaTime, 0));
	}

	void UpdateSpeed()
	{
		cameraSpeed = cameraSpeed*1.001f;
	}
}
