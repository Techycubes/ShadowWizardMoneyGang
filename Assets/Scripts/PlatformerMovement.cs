using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ali Jawad Parpia
public class PlatformerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 14f;
    private bool isJumping = false;

    public int jumpcounter = 0;
    public Animator animator;
    float horizontalMovement = 0f;
    public float maxVelocity = 17f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        horizontalMovement = horizontalInput * moveSpeed;
        Vector2 moveVector = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));

        // Handle jumping
        if (Input.GetButtonDown("Jump") && jumpcounter < 2)
        {
            moveVector.y = jumpForce;
            isJumping = true;
            jumpcounter++;
            animator.SetBool("isJumping", true);
        }

        rb.velocity = moveVector;

        // Flip character sprite based on direction
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

       if( rb.velocity.magnitude > maxVelocity){
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
       }
    }

    // This function is called when the object collides with something
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            jumpcounter = 0;
            animator.SetBool("isJumping", false);
        }
    }
}