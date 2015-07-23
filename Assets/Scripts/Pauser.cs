using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour {
	//this script is used to pause/unpause game and increase game speed over time
	public bool paused = true;
	public float timeScale = 1;
    float camSpeed = 2;
    public Rigidbody2D player;
    MoveUp cameraSpeed;
	
	void Start()
	{
		InvokeRepeating ("IncreaseTimeScale", 1, 15);
        cameraSpeed = Camera.main.GetComponent<MoveUp>();
        camSpeed = MoveUp.cameraSpeed;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.P))
		{
			paused = !paused;
		}

		if(paused)
        {
            MoveUp.cameraSpeed = 0;
            player.isKinematic = true;
        }
		else
        {
            player.isKinematic = false;
            MoveUp.cameraSpeed = camSpeed;
        }
	}

	void IncreaseTimeScale()
	{
		timeScale = timeScale + 0.02f;
	}
}
