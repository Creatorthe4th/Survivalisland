using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float survivalTimeGoal = 120f;
    public TextMeshProUGUI timerText;

    public GameObject winPanel;
    public GameObject losePanel;

    public PlayerHealth playerHealth;

    private float elapsedTime;
    private bool gameOver;

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (playerHealth == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerHealth = playerObj.GetComponent<PlayerHealth>();
            }
        }

        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandleLose;
        }
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (timerText != null)
        {
            float timeRemaining = Mathf.Max(0f, survivalTimeGoal - elapsedTime);
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        if (elapsedTime >= survivalTimeGoal)
        {
            HandleWin();
        }
    }

    private void HandleWin()
    {
        if (gameOver) return;
        gameOver = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HandleLose()
    {
        if (gameOver) return;
        gameOver = true;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandleLose;
        }
    }
}