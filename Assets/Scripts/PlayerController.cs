using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Constants
    private float walkSpeed = 5.1f;
    private float jumpSpeed = 200.1f;
    private float airSpeedMultiplier = 1.4f;

    // Variables
    private bool isOnGround = false;
    private bool canJump = false;
    private Rigidbody2D rb;
    private GameManager gameManagerScript;
    private GameObject gameManagerObject;
    // AudioSource used for jump sound effect
    private AudioSource jumpSoundEffect;

    // These are set in the unity inspector
    public ParticleSystem deathParticles;
    

    // Start is called before the first frame update
    void Start()
    {
        // Load rigid body component
        rb = GetComponent<Rigidbody2D>();
        // Load jump sound effect
        jumpSoundEffect = GetComponent<AudioSource>();

        // Get the game maager from the scene
        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");

        // Load game manager script so it's methods can be called
        gameManagerScript = gameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moving left and right
        float horizontalInput = Input.GetAxis("Horizontal");

        // Slow player movement in the air
        float airMultiplier = 1.0f;
        if (!isOnGround)
        {
            airMultiplier = airSpeedMultiplier;
        }

        rb.AddForce(Vector2.right * horizontalInput * walkSpeed * airMultiplier);

        // Jumping
        float jumpingInput = Input.GetAxis("Vertical");

        // If the input is upwards and the player hasn't jumped...
        if (jumpingInput > 0)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (isOnGround && canJump)
        {
            jumpSoundEffect.Play();
            rb.AddForce(Vector2.up * jumpSpeed);
            canJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            KillPlayer();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            gameManagerScript.GoToNextLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Allow player to jump when they stand on ground
            isOnGround = false;
            canJump = false;
        }
    }

    void KillPlayer()
    {
        // Create death particle effects
        Instantiate(deathParticles, gameObject.transform);

        // Trigger lose logic
        gameManagerScript.Lose();
    }
}
