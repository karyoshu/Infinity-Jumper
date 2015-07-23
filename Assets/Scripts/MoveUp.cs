using UnityEngine;
using System.Collections;

public class MoveUp : MonoBehaviour {
	//this is script is used to move camera upwards
	public static float cameraSpeed = 1f;

	void LateUpdate () {
		transform.Translate(new Vector3(0, cameraSpeed*Time.deltaTime, 0));
	}
}
