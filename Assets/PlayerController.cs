using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 5f;
    public int maxHealth = 100;
    private int currentHealth;
    private int score = 0;
    private float fallThreshold = -5f;
    private int totalCollectibles = 10; // the total number of collectibles

    public TextMeshProUGUI scoreText;
    public Slider healthBar;
    public GameObject gameOverScreen;
    public GameObject restartButton;
    public GameObject exitButton;
    public GameObject winScreen; // Referencing to the "You Won" display

    private Rigidbody rb;
    private bool isGrounded;
    public Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        UpdateScore();

        if (restartButton != null) restartButton.SetActive(false);
        if (exitButton != null) exitButton.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false); // win screen is hidden initially

        if (playerCamera == null) playerCamera = Camera.main;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (transform.position.y < fallThreshold)
        {
            RestartGame();
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = (cameraForward * moveZ + cameraRight * moveX).normalized * speed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movement *= sprintMultiplier;
        }

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(20);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            score += 10;
            Destroy(other.gameObject);
            UpdateScore();

            if (score == totalCollectibles * 10) // If all collectibles are collected
            {
                WinGame();
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            if (score == totalCollectibles * 10)
            {
                scoreText.SetText($"Score: {score}  YOU WON!!");
            }
            else
            {
                scoreText.SetText($"Score: {score}");
            }
        }
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        if (restartButton != null) restartButton.SetActive(true);
        if (exitButton != null) exitButton.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    void WinGame()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        if (restartButton != null) restartButton.SetActive(true);
        if (exitButton != null) exitButton.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
