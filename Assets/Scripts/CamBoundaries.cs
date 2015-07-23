using UnityEngine;
using System.Collections;
//this script update camera's upper and lower boundaries
public class CamBoundaries : MonoBehaviour {

	public Transform killBox;			//camera's lower boundary below which everything is destroyed
	public Transform camUpperLimit;		//camera's upper boundary(player can't go above this)

	Camera mainCam;						//reference to main camera
    public GameObject player;			//reference to player game object
    public ScreenManager screenManager;	//reference to screen manager
	public Animator gameOverCanvas;		//reference to gameOverCanvas animator
	void Awake()
	{
		mainCam = Camera.main;
	}

	void Update()
	{
		//updating camera's upper boundary
		camUpperLimit.position = new Vector2(0f, mainCam.ScreenToWorldPoint (new Vector3( 0f, Screen.height, 0f)).y - 1f);
		//updating camera's lower boundary
		killBox.position = new Vector2 (0f, mainCam.ScreenToWorldPoint (new Vector3( 0f, 0f, 0f)).y - 1f);
		//if player goes below camera's lower boundary, then change game's state to not playing and bring up game over ui
		if (player.transform.position.y < killBox.position.y) {
			GameMaster.gm.ChangeState((int)GameState.NotPlaying);
            screenManager.OpenPanel(gameOverCanvas);
		}
		//if player is trying to go above camera's upper boundary, then move camera 
		if (camUpperLimit.position.y < player.transform.position.y)
		{
			transform.Translate(new Vector3(0, player.transform.position.y - camUpperLimit.position.y, 0));
		}
	}
}
