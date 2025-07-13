using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // the maximum health
    private int currentHealth;
    public GameObject gameOverScreen;
    public GameObject restartButton;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        gameOverScreen.SetActive(false); // Hide Game Over Screen initially
        restartButton.SetActive(false);  // Hide Restart Button initially
        healthBar.value = currentHealth; // Set UI Health Bar
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) // If player hits an obstacle
        {
            TakeDamage(20); // Reduce health by 20
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth; // Update UI Health Bar

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over Triggered"); 

        gameOverScreen.SetActive(true); // Show Game Over Screen
        restartButton.SetActive(true);  // Show Restart Button
        Time.timeScale = 0f; // Pause the game

        // Unlock the cursor so the player can click the restart button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void RestartGame()
    {
        Debug.Log("Restart Button Clicked");

        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload scene

        // Lock the cursor again after the game restarts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
