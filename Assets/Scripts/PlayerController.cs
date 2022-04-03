using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float walkSpeed = 5.1f;
    private float jumpSpeed = 300.1f;
    private float airSpeedMultiplier = 1.1f;

    private bool isOnGround = false;
    private bool canJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (jumpingInput > 0 && isOnGround && canJump)
        {
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

    void KillPlayer() { }
}
