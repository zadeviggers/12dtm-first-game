using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float walkSpeed = 20.1f;
    private float jumpSpeed = 1100.1f;

    private bool canJump = true;


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

        rb.AddForce(Vector2.right * horizontalInput * walkSpeed);

        // Jumping
        float jumpingInput = Input.GetAxis("Vertical");
        
        // If the input is upwards and the player hasn't jumped...
        if (jumpingInput > 0 && canJump)
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

    void KillPlayer() { }
}
