using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Ali Jawad Parpia
public class movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool ifJumping = false;

    // Start is called before the first frame update
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 moveVector = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

    }
}
