using UnityEngine;
using System.Collections;

public class CamBoundaries : MonoBehaviour {

	public Transform killBox;
	public Transform camUpperLimit;

	Camera mainCam;
    public GameObject player;
    public ScreenManager screenManager;
    public Animator gameOverCanvas;
	void Awake()
	{
		mainCam = Camera.main;
	}

	void Update()
	{
		camUpperLimit.position = new Vector2(0f, mainCam.ScreenToWorldPoint (new Vector3( 0f, Screen.height, 0f)).y - 1f);
		killBox.position = new Vector2 (0f, mainCam.ScreenToWorldPoint (new Vector3( 0f, 0f, 0f)).y - 1f);
		if (player.transform.position.y < killBox.position.y) {
			GameMaster.gm.ChangeState((int)GameState.NotPlaying);
            screenManager.OpenPanel(gameOverCanvas);
		}
		if (camUpperLimit.position.y < player.transform.position.y)
		{
			transform.Translate(new Vector3(0, player.transform.position.y - camUpperLimit.position.y, 0));
		}
	}
}
