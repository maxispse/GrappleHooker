using UnityEngine;
using TMPro;  // add this at top

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxLives = 3;
    public int currentLives = 3;

    [Header("Shield Settings")]
    public GameObject ShieldSprite;
    private bool hasShield = false;

    [Header("UI Elements")]
    public TMP_Text livesText;
    private Animator animator; // optional, for hurt animation

    public PlayerHealthUI playerHealthUI;

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
        animator = GetComponent<Animator>();
    }

    public void OnShieldButton()
    {
        if (!hasShield)
        {
            ActivateShield();
        }
    }

    // Call this to apply damage
    public void TakeDamage(int damage = 1)
    {
        if (hasShield)
        {
            hasShield = false;
            maxLives -= damage;
            currentLives -= damage;
            UpdateLivesUI();
            Debug.Log("Shield absorbed the hit!");
            OnShieldBreak();
            return;
        }

        if (!hasShield)
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
    void ActivateShield()
    {
        hasShield = true;
        maxLives += 1;
        currentLives = maxLives;
        UpdateLivesUI();
        if (ShieldSprite != null)
        {
            ShieldSprite.SetActive(true);
        }
        Debug.Log("Shield activated!");
    }
    void OnShieldBreak()
    {
        ShieldSprite.SetActive(false);
    }
    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "" + currentLives;
    }
}
