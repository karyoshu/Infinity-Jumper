using UnityEngine;
using System.Collections;

public class SoundSetup : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		audio.volume = PlayerPrefs.GetFloat ("SoundLevel");
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		audio.volume = PlayerPrefs.GetFloat ("SoundLevel");
	}
}
