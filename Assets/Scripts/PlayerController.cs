using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Constants
    private float walkSpeed = 5.1f;
    private float jumpSpeed = 200.1f;
    private float airSpeedMultiplier = 1.3f; // Speed multiplier for moving mid-air

    // Movement Variables
    private bool isOnGround = false;
    private bool canJump = false;
    // Other variables
    private Rigidbody2D rb;
    private GameManager gameManagerScript;
    private GameObject gameManagerObject;

    // AudioSource used for jump sound effect
    private AudioSource[] jumpSoundEffects;

    // These are set in the unity inspector
    public ParticleSystem deathParticles;
    

    // Start is called before the first frame update
    void Start()
    {
        // Load rigid body component
        rb = GetComponent<Rigidbody2D>();
        // Load jump sound effect
        jumpSoundEffects = GetComponents<AudioSource>();

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

    // Called when the player enters a trigger
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

    // Called when the player starts colliding with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            canJump = true;
        }
    }

    // Called when the player stops colliding with something
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Allow player to jump when they stand on ground
            isOnGround = false;
            canJump = false;
        }
    }

    // Method to make the player jump
    void Jump()
    {
        // Check that the player is allowed to jump
        if (isOnGround && canJump)
        {
            PlayJumpSoundEffect();
            // Add force
            rb.AddForce(Vector2.up * jumpSpeed);
            canJump = false;
        }
    }

    // Method to play the jump sound effect
    void PlayJumpSoundEffect()
    {
        // Choose a random jump sound effect from the ones available
        int soundEffectIndex = Random.Range(0, jumpSoundEffects.Length);
        AudioSource soundEffect = jumpSoundEffects[soundEffectIndex];
        
        // Play sound effect
        soundEffect.Play();
    }

    // Kill the player
    void KillPlayer()
    {
        // Create death particle effects
        Instantiate(deathParticles, gameObject.transform);

        // Hide player by disabling sprite renderer
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Trigger lose logic
        gameManagerScript.Lose();
    }
}
