using UnityEngine;
using System.Collections;

public class KillAll : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy (col.collider.gameObject);
	}
}