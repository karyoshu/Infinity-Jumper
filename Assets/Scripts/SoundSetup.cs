using UnityEngine;
using System.Collections;

public class SoundSetup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat ("SoundLevel");
		GetComponent<AudioSource>().Play ();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat ("SoundLevel");
	}
}
