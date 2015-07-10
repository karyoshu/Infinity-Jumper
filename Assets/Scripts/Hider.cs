using UnityEngine;
using System.Collections;

public class Hider : MonoBehaviour {
	GameObject player;
	PlayerControl playerControl;
	SpriteRenderer spriteRenderer;
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	void OnTriggerEnter2D()
	{
		if(player.rigidbody2D.velocity.y < 0)
		{
			playerControl.SendMessage ("Jump");
		}
	}

	void HideMe()
	{
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float myX = transform.position.x;
        float myY = transform.position.y;

        float distance = Mathf.Sqrt((playerX - myX) * (playerX - myX) + (playerY - myY) * (playerY - myY));
		if (distance < 0)
			distance = -distance;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1 / (distance * distance));
	}

	void Update()
	{
		HideMe ();
	}
}