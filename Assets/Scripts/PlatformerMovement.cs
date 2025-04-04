using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerMovement : MonoBehaviour
{
    public bool isChkpt1Touch;
    public bool isChkpt2Touch;
    public bool isChkpt3Touch;
    public bool isChkpt4Touch;
    public bool isChkpt5Touch;
    public bool isChkpt6Touch;
    public bool isChkpt7Touch;
    public bool isChkpt8Touch;
    public string carName = "Default"; // Local variable (no need to make it public unless Inspector needs it)
    private Rigidbody2D rb;
    public float moveSpeed;          // Target maximum speed
    public float acceleration;      // How quickly speed builds up
    public float deceleration;       // How quickly speed slows down
    public float rotationSpeed;    // Degrees per second
    public float maxVelocity;
    public bool Oiled;
    private Vector2 currentVelocity;
  //      public Animator animator;
        private SpriteRenderer spriteRenderer;
        public Sprite newSprite;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        carName = Selectionmenu.CarName; // Access the static CarName directly
        currentVelocity = Vector2.zero;
        Oiled = false;
        isChkpt1Touch = false;
        isChkpt2Touch = false;
        isChkpt3Touch = false;
        isChkpt4Touch = false;
        isChkpt5Touch = false;
        isChkpt6Touch = false;
        isChkpt7Touch = false;
        isChkpt8Touch = false;
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
            spriteRenderer.sprite = newSprite;
            moveSpeed = 10f;
            acceleration = 9f;
            deceleration = 10f;
            rotationSpeed = 160f;
            maxVelocity = 15f;
        }else if (carName == "Car3")
        {
            moveSpeed = 11f;
            acceleration = 10f;
            deceleration = 8f;
            rotationSpeed = 170f;
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
        if(rotationSpeed >= 0){
            float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }else if (rotationSpeed < 0){
            float rotationSpeed = 0;
        }

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
    void OnCollisionEnter2D(Collision2D collision){
        switch(collision.gameObject.tag){
            case "Ob1":
                
                Debug.Log("Ob1");
                break;
            case "Ob2":
                
                Debug.Log("Ob2");
                break;
            case "Ob3":
                
                Debug.Log("Ob3");
                break;
            case "Ob4":
                
                Debug.Log("Ob4");
                break;
            
        }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        
        switch(collision.gameObject.tag){
            case "Chkpt1":
                isChkpt1Touch = true;
                Debug.Log("c1t");
                break;
            case "Chkpt2":
                isChkpt2Touch = true;
                Debug.Log("c2t");
                break;
            case "Chkpt3":
                isChkpt3Touch = true;
                Debug.Log("c3t");
                break;
            case "Chkpt4":
                isChkpt4Touch = true;
                Debug.Log("c4t");
                break;
            case "Chkpt5":
                isChkpt5Touch = true;
                Debug.Log("c5t");
                break;
            case "Chkpt6":
                isChkpt6Touch = true;
                Debug.Log("c6t");
                break;
            case "Chkpt7":
                isChkpt7Touch = true;
                Debug.Log("c7t");
                break;
            case "Chkpt8":
                isChkpt8Touch = true;
                Debug.Log("c8t");
                break;
        }
        if(collision.gameObject.CompareTag("Ob4") && !Oiled){
            rotationSpeed -= 130;
            deceleration -= 2;
            Oiled = true;
            StartCoroutine(OilEffectTimer());
        }
        if (collision.gameObject.CompareTag("End") && isChkpt1Touch && isChkpt2Touch && isChkpt3Touch && isChkpt4Touch && isChkpt5Touch && isChkpt6Touch && isChkpt7Touch && isChkpt8Touch)
        {
            Debug.Log("a");
            SceneManager.LoadScene("Results");
        }
    }
        private IEnumerator OilEffectTimer()
    {

        yield return new WaitForSeconds(5f); // Wait for 4 seconds
        
        // Reset values to original
        rotationSpeed += 130;
        deceleration += 2;
        Oiled = false;
    }
}

