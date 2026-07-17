using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    public AudioSource audioSource;
    public AudioClip deathSound;

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        GetComponent<EnemyAI>().enabled = false;

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Give the death sound time to play before removing the enemy
        float delay = deathSound != null ? deathSound.length : 0f;
        Destroy(gameObject, delay);
    }
}