using UnityEngine;
using TMPro;  // add this at top

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxLives = 3;
    public int currentLives;
    public TMP_Text livesText;
    private Animator animator; // optional, for hurt animation

    [Header("UI Settings")]
    public PlayerHealthUI playerHealthUI;

    void Start()
    {
        currentLives = maxLives;
        animator = GetComponent<Animator>();
    }

    // Call this to apply damage
    public void TakeDamage(int damage = 1)
    {
        currentLives -= damage;
        currentLives = Mathf.Max(currentLives, 0);

        Debug.Log("Player hit! Lives remaining: " + currentLives);

        if (playerHealthUI != null)
            playerHealthUI.FlashHit();
        UpdateLivesUI();
        // Optional: play hurt animation
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (currentLives <= 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            TakeDamage(1);
        }
    }
    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "" + currentLives;
    }
}
