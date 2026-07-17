using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;

    void Start()
    {
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
            playerHealth.OnHealthChanged += UpdateHealthText;
            UpdateHealthText(playerHealth.currentHealth, playerHealth.maxHealth);
        }
    }

    private void UpdateHealthText(int current, int max)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {current}/{max}";
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthText;
        }
    }
}