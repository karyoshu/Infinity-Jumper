using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	Animator anim;						//reference to Animator component on player
	public float jumpVel = 7;			//jump velocity by which player goes up
	public bool isGrounded = true;		//to check if player touched the ground or not
	public float moveSpeed = 200;		//player movement speed sideways
	public AudioClip[] jump;			//array containing jump audio clips
    public static bool Boosted = false;	//to check if player was recently boosted or not
	// Use this for initialization
	void Awake () {
		anim = transform.GetComponent<Animator> ();
	}

	
	// Update is called once per frame
	void FixedUpdate () {
        #if CROSS_PLATFORM_INPUT
            float move = CrossPlatformInput.GetAxis("Horizontal");
        #else
            float move = Input.GetAxis("Horizontal");
        #endif
        rigidbody2D.velocity = new Vector2 (move * moveSpeed * Time.fixedDeltaTime, rigidbody2D.velocity.y);
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		anim.SetBool ("isGrounded", isGrounded);
	}

	void Jump()
	{
		//add positive velocity in y-axis
		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpVel * Time.deltaTime);
		isGrounded = false;		//make isGrounded parameter to false
		audio.clip = jump [Random.Range (0, jump.Length)];		//play a random jump sound with a random pitch
		audio.pitch = Random.Range(0.9f,1.1f);
		audio.Play();
	}

    IEnumerator Boost()
	{
		//boosts the player on taking green ball
        if(rigidbody2D.velocity.y < 0)
		    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20);
        else
            rigidbody2D.velocity += new Vector2(rigidbody2D.velocity.x, 20);
		isGrounded = false;
        Boosted = true;
        yield return new WaitForSeconds(4);
        Boosted = false;
	}

    IEnumerator SuperBoost()
	{
		//super boosts player on taking blue ball
        if (rigidbody2D.velocity.y < 0)
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 30);
        else
            rigidbody2D.velocity += new Vector2(rigidbody2D.velocity.x, 30);
        isGrounded = false;
        Boosted = true;
        yield return new WaitForSeconds(6);
        Boosted = false;
	}

	IEnumerator InvertControl()
	{
		//invert player controls
		Invert ();
        yield return new WaitForSeconds(5);
        if (moveSpeed < 0)
            Invert();
	}

	void Invert()
	{
		moveSpeed = -moveSpeed;
	}

	void OnCollisionEnter2D()
	{
		isGrounded = true;
	}

    public void ToggleSound()
    {
        audio.enabled = !audio.enabled;
    }
}
