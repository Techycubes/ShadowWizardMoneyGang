using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformerMovement : MonoBehaviour
{
    private List<Vector2> StoredPositions = new List<Vector2>();
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
  //public Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite newSprite2;
    public Sprite newSprite3;
    public bool IsSpunOut;
    public int SpinOutLives;
    float horizontalInput;
    float verticalInput;
    public GameObject targetObject;
    bool hasStartedLogging;
    void Start()
    {
        SpinOutLives = 3;
        IsSpunOut = false;
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
            spriteRenderer.sprite = newSprite3;
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
            spriteRenderer.sprite = newSprite2;
            moveSpeed = 11f;
            acceleration = 10f;
            deceleration = 8f;
            rotationSpeed = 170f;
            maxVelocity = 15f;
        }

    }


    void Update()
    {
        if (DissapearObject.CanStart && !hasStartedLogging)
        {
        StartCoroutine(LogPositions());
        hasStartedLogging = true;
        StartCoroutine(Wait60s());
        }
        if(DissapearObject.CanStart && !IsSpunOut && SpinOutLives>0){
        // Get input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

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


      }else if(IsSpunOut){
        horizontalInput = 0f;
        verticalInput = 0f;
      }else if(SpinOutLives<=0){
        Debug.Log("0 lives");
        SceneManager.LoadScene("Results 1");
      }
    }
    void OnCollisionEnter2D(Collision2D collision){
    switch(collision.gameObject.tag)
        {
            case "Ob1":
                Debug.Log("Ob1");
                if (!IsSpunOut){
                    StartCoroutine(SpinOutEffect());
                    SpinOutLives--;
                }
                break;
            case "Ob2":
                Debug.Log("Ob2");
                if (!IsSpunOut){
                    StartCoroutine(SpinOutEffect());
                    SpinOutLives--;
                }
                break;
            case "Ob3":
                Debug.Log("Ob3");
                if (!IsSpunOut)
                {
                    StartCoroutine(SpinOutEffect());
                    SpinOutLives--;
                }
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
        if(collision.gameObject.CompareTag("Ob4") && !Oiled && rotationSpeed>130){
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
    private IEnumerator SpinOutEffect()
    {
        currentVelocity = Vector2.zero;
            horizontalInput = 0f;
        verticalInput = 0f;
        IsSpunOut = true;
        
        // Store original values
        float originalMoveSpeed = moveSpeed;
        float originalRotationSpeed = rotationSpeed;
        
        // Disable physics movement completely
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        // Stop movement and set spinning
        moveSpeed = 0f;
        rotationSpeed = 360f;
        
        // Calculate backward movement
        Vector2 backwardDirection = -transform.up;
        float backwardDistance = 1f; // How far to move back (adjust as needed)
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + (backwardDirection * backwardDistance);
        float backwardDuration = 0.5f; // Time for backward movement
        
        // Move backward smoothly
        float elapsedTime = 0f;
        while (elapsedTime < backwardDuration)
            {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / backwardDuration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
            }
        transform.position = targetPosition; // Ensure exact final position
        
        // Spin in place for remaining time
        yield return new WaitForSeconds(0.5f); // Total 3s with 0.5s backward movement
        
        // Restore original state
        IsSpunOut = false;
        moveSpeed = originalMoveSpeed;
        rotationSpeed = originalRotationSpeed;
        rb.velocity = Vector2.zero; // Ensure no residual velocity
        rb.angularVelocity = 0f;
        horizontalInput = 0f;
        verticalInput = 0f;
    }
    IEnumerator LogPositions(){
            for (int i = 0; i < 600; i++)
            {
                Vector2 currentPosition = targetObject.transform.position;
                StoredPositions.Add(currentPosition);
                yield return new WaitForSeconds(0.1f);
                Debug.Log(StoredPositions[i]);
            }
    }
    IEnumerator Wait60s(){
        yield return new WaitForSeconds(60f);
        Debug.Log("60S");
    }
}