using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlatformerMovement : MonoBehaviour
{
    private List<Vector2> StoredPositions = new List<Vector2>();
    private float raceTime;
    private bool isRaceFinished;
    private string savePath;
    public bool isChkpt1Touch;
    public bool isChkpt2Touch;
    public bool isChkpt3Touch;
    public bool isChkpt4Touch;
    public bool isChkpt5Touch;
    public bool isChkpt6Touch;
    public bool isChkpt7Touch;
    public bool isChkpt8Touch;
    public string carName = "Default";
    private Rigidbody2D rb;
    public float moveSpeed;
    public float acceleration;
    public float deceleration;
    public float rotationSpeed;
    public float maxVelocity;
    public bool Oiled;
    private Vector2 currentVelocity;
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

    [System.Serializable]
    public class HighScoreData
    {
        public string carName;
        public float bestTime;
        public List<Vector2> bestRunPositions;
    }

    void Start()
    {
        savePath = Application.persistentDataPath + "/highscore.json";
        SpinOutLives = 3;
        IsSpunOut = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        carName = Selectionmenu.CarName;
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
        raceTime = 0f;
        isRaceFinished = false;

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
        }
        else if (carName == "Car3")
        {
            spriteRenderer.sprite = newSprite2;
            moveSpeed = 11f;
            acceleration = 10f;
            deceleration = 8f;
            rotationSpeed = 170f;
            maxVelocity = 15f;
        }

        LoadHighScore();
    }

    void FixedUpdate()
    {
        if (DissapearObject.CanStart && !isRaceFinished)
        {
            raceTime += Time.deltaTime;
        }

        if (DissapearObject.CanStart && !hasStartedLogging)
        {
            StartCoroutine(LogPositions());
            hasStartedLogging = true;
            StartCoroutine(Wait60s());
        }

        if (DissapearObject.CanStart && !IsSpunOut && SpinOutLives > 0)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

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
            if (rotationSpeed >= 0)
            {
                float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, 0, rotationAmount);
            }
            else
            {
                rotationSpeed = 0;
            }

            Vector2 forwardDirection = transform.up;
            Vector2 targetVelocity = forwardDirection * verticalInput * moveSpeed;

            if (verticalInput != 0)
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
            }

            rb.velocity = currentVelocity;

            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
            }
        }
        else if (IsSpunOut)
        {
            horizontalInput = 0f;
            verticalInput = 0f;
        }
        else if (SpinOutLives <= 0)
        {
            Debug.Log("0 lives");
            SceneManager.LoadScene("Results 1");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ob1":
            case "Ob2":
            case "Ob3":
                Debug.Log(collision.gameObject.tag);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
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

        if (collision.gameObject.CompareTag("Ob4") && !Oiled && rotationSpeed > 130)
        {
            rotationSpeed -= 130;
            deceleration -= 2;
            Oiled = true;
            StartCoroutine(OilEffectTimer());
        }

        if (collision.gameObject.CompareTag("End") && isChkpt1Touch && isChkpt2Touch && 
            isChkpt3Touch && isChkpt4Touch && isChkpt5Touch && isChkpt6Touch && 
            isChkpt7Touch && isChkpt8Touch)
        {
            isRaceFinished = true;
            SaveHighScore();
            SceneManager.LoadScene("Results");
        }
    }

    private IEnumerator OilEffectTimer()
    {
        yield return new WaitForSeconds(5f);
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

        float originalMoveSpeed = moveSpeed;
        float originalRotationSpeed = rotationSpeed;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        moveSpeed = 0f;
        rotationSpeed = 360f;

        Vector2 backwardDirection = -transform.up;
        float backwardDistance = 1f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + (backwardDirection * backwardDistance);
        float backwardDuration = 0.5f;

        float elapsedTime = 0f;
        while (elapsedTime < backwardDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / backwardDuration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        transform.position = targetPosition;

        yield return new WaitForSeconds(0.5f);

        IsSpunOut = false;
        moveSpeed = originalMoveSpeed;
        rotationSpeed = originalRotationSpeed;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        horizontalInput = 0f;
        verticalInput = 0f;
    }

    IEnumerator LogPositions()
    {
        StoredPositions.Clear();
        for (int i = 0; i < 600; i++)
        {
            Vector2 currentPosition = targetObject.transform.position;
            StoredPositions.Add(currentPosition);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Wait60s()
    {
        yield return new WaitForSeconds(60f);
        Debug.Log("60S");
    }

    void SaveHighScore()
    {
        Debug.Log("Saving high score...");
        HighScoreData data = LoadHighScore();
        if (data == null || raceTime < data.bestTime || data.bestTime == 0)
        {
            data = new HighScoreData
            {
                carName = carName,
                bestTime = raceTime,
                bestRunPositions = new List<Vector2>(StoredPositions)
            };
            string json = JsonUtility.ToJson(data);
            Debug.Log($"Saving JSON: {json}");
            File.WriteAllText(savePath, json);
            Debug.Log($"Saved high score: {raceTime} seconds with {carName}");
        }
    }

    HighScoreData LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<HighScoreData>(json);
        }
        return null;
    }

    public List<Vector2> GetStoredPositions()
    {
        return StoredPositions;
    }

    public float GetRaceTime()
    {
        return raceTime;
    }

    public HighScoreData GetHighScoreData()
    {
        return LoadHighScore();
    }
}