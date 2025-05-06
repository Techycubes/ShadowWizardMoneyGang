using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlatformerMovement : MonoBehaviour
{
    public Animator animator;
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
    public int coins; // Total coin count
    private int Direction; // Animation direction (-1: left, 1: right, 0: idle)
    private HighScoreData cachedHighScoreData;

    [System.Serializable]
    public class HighScoreData
    {
        public int coins; // Total coins across all levels
        public bool isCar2Purchased; // Persist Car2 purchase
        public bool isCar3Purchased; // Persist Car3 purchase
        public bool hasPlayed; // Track if game has been played
        public List<LevelData> levels = new List<LevelData>();
    }

    [System.Serializable]
    public class LevelData
    {
        public string level;
        public string carName;
        public float bestTime;
        public List<Vector2> bestRunPositions;
    }

    void Awake()
    {
        savePath = Path.Combine("C:/Formula2Game", "highscore.json");
        Debug.Log($"Awake: savePath={savePath}, writable={IsPathWritable(savePath)}");
        LoadHighScore();

        // Check if this is the first time playing
        if (!cachedHighScoreData.hasPlayed)
        {
            Debug.Log("First time playing, loading Introduction scene");
            cachedHighScoreData.hasPlayed = true;
            SaveToFile(cachedHighScoreData); // Save hasPlayed = true
            SceneManager.LoadScene("Introduction");
        }
    }

    void Start()
    {
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
        coins = cachedHighScoreData?.coins ?? 0;
        Direction = 0; // Initialize direction

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
            acceleration = 6f;
            deceleration = 8f;
            rotationSpeed = 170f;
            maxVelocity = 15f;
        }
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
            if (rotationSpeed < 0)
            {
                rotationSpeed = 0;
            }

            // Rotate the car
            float rotationAmount = -horizontalInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);

            // Set animation direction
            if (Input.GetKey("a") && !Input.GetKey("d"))
            {
                Direction = -1; // Left
            }
            else if (Input.GetKey("d") && !Input.GetKey("a"))
            {
                Direction = 1; // Right
            }
            else
            {
                Direction = 0; // Idle
            }
            animator.SetInteger("TurnDirection", Direction);

            // Raycast to prevent phasing
            Vector2 rayDirection = verticalInput >= 0 ? transform.up : -transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1f);
            if (hit.collider != null && (hit.collider.CompareTag("Ob1") || hit.collider.CompareTag("Ob2") || hit.collider.CompareTag("Ob3")))
            {
                rb.velocity = Vector2.zero;
                currentVelocity = Vector2.zero;
            }
            else
            {
                // Calculate velocity
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

                // Clamp velocity
                if (rb.velocity.magnitude > maxVelocity)
                {
                    rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
                }
            }
        }
        else if (IsSpunOut)
        {
            horizontalInput = 0f;
            verticalInput = 0f;
            Direction = 0;
            animator.SetInteger("TurnDirection", 0);
            rb.velocity = Vector2.zero;
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
                Debug.Log($"Collision with {collision.gameObject.tag}, Velocity: {rb.velocity}, VerticalInput: {verticalInput}");
                if (!IsSpunOut && SpinOutLives > 0)
                {
                    StartCoroutine(SpinOutEffect());
                    SpinOutLives--;
                }
                rb.velocity = Vector2.zero;
                currentVelocity = Vector2.zero;
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

        Vector2 movementDirection = verticalInput >= 0 ? -transform.up : transform.up;
        float moveDistance = 1.5f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + (movementDirection * moveDistance);
        float moveDuration = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(startPosition, movementDirection, moveDistance);
        if (hit.collider != null && (hit.collider.CompareTag("Ob1") || hit.collider.CompareTag("Ob2") || hit.collider.CompareTag("Ob3")))
        {
            targetPosition = startPosition + (movementDirection * hit.distance * 0.9f);
        }

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t);
            rb.MovePosition(newPosition);
            yield return null;
        }
        rb.MovePosition(targetPosition);

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
        Debug.Log("Saving high score and coins...");
        HighScoreData data = cachedHighScoreData ?? new HighScoreData();
        string currentLevel = SceneManager.GetActiveScene().name;
        int newCoins = 3 + Mathf.FloorToInt(1000f / raceTime);

        data.coins += newCoins;
        data.isCar2Purchased = Purchase.is2Purchase;
        data.isCar3Purchased = Purchase.is3Purchase;
        data.hasPlayed = true; // Ensure hasPlayed remains true

        LevelData levelData = data.levels.Find(ld => ld.level == currentLevel);
        if (levelData == null)
        {
            levelData = new LevelData
            {
                level = currentLevel,
                carName = carName,
                bestTime = raceTime,
                bestRunPositions = new List<Vector2>(StoredPositions)
            };
            data.levels.Add(levelData);
        }
        else
        {
            if (raceTime < levelData.bestTime || levelData.bestTime == 0)
            {
                levelData.carName = carName;
                levelData.bestTime = raceTime;
                levelData.bestRunPositions = new List<Vector2>(StoredPositions);
            }
        }

        cachedHighScoreData = data;
        SaveToFile(data);
    }

    HighScoreData LoadHighScore()
    {
        if (cachedHighScoreData != null)
        {
            Debug.Log($"Returning cached HighScoreData, coins: {cachedHighScoreData.coins}, Car2Purchased: {cachedHighScoreData.isCar2Purchased}, Car3Purchased: {cachedHighScoreData.isCar3Purchased}, HasPlayed: {cachedHighScoreData.hasPlayed}");
            coins = cachedHighScoreData.coins;
            Purchase.is2Purchase = cachedHighScoreData.isCar2Purchased;
            Purchase.is3Purchase = cachedHighScoreData.isCar3Purchased;
            return cachedHighScoreData;
        }

        Debug.Log($"Loading high score from: {savePath}");
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("highscore.json does not exist, creating with 117 coins and hasPlayed=false");
            HighScoreData defaultData = new HighScoreData { coins = 117, hasPlayed = false };
            cachedHighScoreData = defaultData;
            SaveToFile(defaultData);
            coins = defaultData.coins;
            Purchase.is2Purchase = defaultData.isCar2Purchased;
            Purchase.is3Purchase = defaultData.isCar3Purchased;
            return defaultData;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            Debug.Log($"Read JSON: {json}");
            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning("highscore.json is empty, creating with 117 coins and hasPlayed=false");
                HighScoreData defaultData = new HighScoreData { coins = 117, hasPlayed = false };
                cachedHighScoreData = defaultData;
                SaveToFile(defaultData);
                coins = defaultData.coins;
                Purchase.is2Purchase = defaultData.isCar2Purchased;
                Purchase.is3Purchase = defaultData.isCar3Purchased;
                return defaultData;
            }

            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            if (data == null)
            {
                Debug.LogError("Failed to deserialize highscore.json, creating with 117 coins and hasPlayed=false");
                HighScoreData defaultData = new HighScoreData { coins = 117, hasPlayed = false };
                cachedHighScoreData = defaultData;
                SaveToFile(defaultData);
                coins = defaultData.coins;
                Purchase.is2Purchase = defaultData.isCar2Purchased;
                Purchase.is3Purchase = defaultData.isCar3Purchased;
                return defaultData;
            }

            cachedHighScoreData = data;
            coins = data.coins;
            Purchase.is2Purchase = data.isCar2Purchased;
            Purchase.is3Purchase = data.isCar3Purchased;
            Debug.Log($"Loaded coins: {data.coins}, Car2Purchased: {data.isCar2Purchased}, Car3Purchased: {data.isCar3Purchased}, HasPlayed: {data.hasPlayed}");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading highscore.json: {e.Message}, StackTrace: {e.StackTrace}");
            HighScoreData defaultData = new HighScoreData { coins = 117, hasPlayed = false };
            cachedHighScoreData = defaultData;
            SaveToFile(defaultData);
            coins = defaultData.coins;
            Purchase.is2Purchase = defaultData.isCar2Purchased;
            Purchase.is3Purchase = defaultData.isCar3Purchased;
            return defaultData;
        }
    }

    private void SaveToFile(HighScoreData data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log($"Saving JSON: {json}");

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(json);
                    writer.Flush();
                }
            }
            if (File.Exists(savePath))
            {
                string verifyJson = File.ReadAllText(savePath);
                FileInfo fileInfo = new FileInfo(savePath);
                Debug.Log($"Successfully saved highscore.json at {savePath}, exists: {File.Exists(savePath)}, content: {verifyJson}, attributes: {fileInfo.Attributes}, last write: {fileInfo.LastWriteTime}");
            }
            else
            {
                Debug.LogWarning($"File not found after write at {savePath}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save highscore.json at {savePath}: {e.Message}, StackTrace: {e.StackTrace}");
            string fallbackPath = Application.persistentDataPath + "/highscore.json";
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fallbackPath));
                File.WriteAllText(fallbackPath, json);
                if (File.Exists(fallbackPath))
                {
                    string verifyJson = File.ReadAllText(fallbackPath);
                    Debug.Log($"Successfully saved highscore.json at fallback {fallbackPath}, exists: {File.Exists(fallbackPath)}, content: {verifyJson}");
                    savePath = fallbackPath;
                }
            }
            catch (System.Exception fallbackEx)
            {
                Debug.LogError($"Failed to save highscore.json at fallback {fallbackPath}: {fallbackEx.Message}, StackTrace: {fallbackEx.StackTrace}");
            }
        }
    }

    public void UpdateCoins(int newCoinTotal)
    {
        Debug.Log($"UpdateCoins: Setting coins to {newCoinTotal}, Car2Purchased: {Purchase.is2Purchase}, Car3Purchased: {Purchase.is3Purchase}, HasPlayed: {cachedHighScoreData.hasPlayed}");
        coins = newCoinTotal;
        HighScoreData data = cachedHighScoreData ?? new HighScoreData();
        data.coins = coins;
        data.isCar2Purchased = Purchase.is2Purchase;
        data.isCar3Purchased = Purchase.is3Purchase;
        data.hasPlayed = true; // Ensure hasPlayed remains true
        cachedHighScoreData = data;
        SaveToFile(data);
    }

    public List<Vector2> GetStoredPositions()
    {
        return StoredPositions;
    }

    public float GetRaceTime()
    {
        return raceTime;
    }

    public int GetCoins()
    {
        return coins;
    }

    public HighScoreData GetHighScoreData()
    {
        return LoadHighScore();
    }

    private bool IsPathWritable(string path)
    {
        try
        {
            string testPath = Path.Combine(Path.GetDirectoryName(path), "test.txt");
            File.WriteAllText(testPath, "test");
            File.Delete(testPath);
            return true;
        }
        catch
        {
            return false;
        }
    }
}