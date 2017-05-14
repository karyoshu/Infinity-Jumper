using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	Animator anim;						//reference to Animator component on player
	public float jumpVel = 7;			//jump velocity by which player goes up
	public bool isGrounded = true;		//to check if player touched the ground or not
	public float moveSpeed = 200;		//player movement speed sideways
	public AudioClip[] jump;			//array containing jump audio clips
    public static bool Boosted = false; //to check if player was recently boosted or not
                                        // Use this for initialization

    Rigidbody2D rb;
    AudioSource audioSource;

	void Awake () {
		anim = transform.GetComponent<Animator> ();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
	}

	
	// Update is called once per frame
	void FixedUpdate () {
        #if CROSS_PLATFORM_INPUT
            float move = CrossPlatformInput.GetAxis("Horizontal");
        #else
            float move = Input.GetAxis("Horizontal");
        #endif
        rb.velocity = new Vector2 (move * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
		anim.SetFloat ("vSpeed", rb.velocity.y);
		anim.SetBool ("isGrounded", isGrounded);
	}

	void Jump()
	{
		//add positive velocity in y-axis
		rb.velocity = new Vector2(rb.velocity.x, jumpVel * Time.deltaTime);
		isGrounded = false;		//make isGrounded parameter to false
		audioSource.clip = jump [Random.Range (0, jump.Length)];		//play a random jump sound with a random pitch
		audioSource.pitch = Random.Range(0.9f,1.1f);
		audioSource.Play();
	}

    IEnumerator Boost()
	{
		//boosts the player on taking green ball
        if(rb.velocity.y < 0)
		    rb.velocity = new Vector2(rb.velocity.x, 20);
        else
            rb.velocity += new Vector2(rb.velocity.x, 20);
		isGrounded = false;
        Boosted = true;
        yield return new WaitForSeconds(4);
        Boosted = false;
	}

    IEnumerator SuperBoost()
	{
		//super boosts player on taking blue ball
        if (rb.velocity.y < 0)
            rb.velocity = new Vector2(rb.velocity.x, 30);
        else
            rb.velocity += new Vector2(rb.velocity.x, 30);
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
        audioSource.enabled = !audioSource.enabled;
    }
}
