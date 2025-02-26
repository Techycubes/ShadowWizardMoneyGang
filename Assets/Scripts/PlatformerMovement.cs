using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ali Jawad Parpia
public class PlatformerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 20f;          // Target maximum speed
    public float acceleration = 10f;      // How quickly speed builds up
    public float deceleration = 8f;       // How quickly speed slows down
    public float rotationSpeed = 180f;    // Degrees per second
    public float maxVelocity = 17f;
    private Vector2 currentVelocity;      // Track current velocity

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentVelocity = Vector2.zero;
    }

    void Update()
    {
        // Get input
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down arrows

        // Handle rotation
        float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationAmount);

        // Calculate target velocity
        Vector2 forwardDirection = transform.up;
        Vector2 targetVelocity = forwardDirection * verticalInput * moveSpeed;

        // Apply acceleration/deceleration
        if (verticalInput != 0)
        {
            // Accelerate towards target velocity
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            // Decelerate when no input
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.deltaTime
            );
        }

        // Apply velocity to Rigidbody
        rb.velocity = currentVelocity;

        // Cap max velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }
}
/*
old script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ali Jawad Parpia
public class PlatformerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f; // Degrees per second
    public float maxVelocity = 17f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float verticalInput = Input.GetAxis("Vertical");     // W/S or Up/Down arrows

        // Handle rotation
        float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationAmount);

        // Calculate movement direction based on current rotation
        Vector2 forwardDirection = transform.up; // In 2D, "up" is the forward direction for sprites
        Vector2 movement = forwardDirection * verticalInput * moveSpeed;

        // Apply velocity
        rb.velocity = movement;

        // Cap max velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }
}*/