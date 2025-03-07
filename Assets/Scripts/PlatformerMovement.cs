using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    public string carName = "Default"; // Local variable (no need to make it public unless Inspector needs it)
    private Rigidbody2D rb;
    public float moveSpeed;          // Target maximum speed
    public float acceleration;      // How quickly speed builds up
    public float deceleration;       // How quickly speed slows down
    public float rotationSpeed;    // Degrees per second
    public float maxVelocity;
    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        carName = Selectionmenu.CarName; // Access the static CarName directly
        currentVelocity = Vector2.zero;

        // Set stats based on carName
        if (carName == "Default")
        {
            moveSpeed = 12f;
            acceleration = 8f;
            deceleration = 8f;
            rotationSpeed = 180f;
            maxVelocity = 17f;
        }
        else if (carName == "Car2")
        {
            moveSpeed = 10f;
            acceleration = 9f;
            deceleration = 10f;
            rotationSpeed = 160f;
            maxVelocity = 15f;
        }
    }

    void Update()
    {
        // Get input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Handle rotation
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rotationSpeed += 50f;
            maxVelocity += 1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rotationSpeed -= 50f;
            maxVelocity -= 1f;
        }
        float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationAmount);

        // Calculate target velocity
        Vector2 forwardDirection = transform.up;
        Vector2 targetVelocity = forwardDirection * verticalInput * moveSpeed;

        // Apply acceleration/deceleration
        if (verticalInput != 0)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
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