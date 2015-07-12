using UnityEngine;
using System.Collections;

public class MoveUp : MonoBehaviour {

	public static float cameraSpeed = 1f;

	void LateUpdate () {
		transform.Translate(new Vector3(0, cameraSpeed*Time.deltaTime, 0));
	}

}
