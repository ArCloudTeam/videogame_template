using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3;
    public float invulnerabilityTime = 1f;

    private int currentHealth;
    private SpriteRenderer sprite;
    private float invulnerabilityTimer;
    private bool isInvulnerable;

    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            sprite.enabled = Mathf.FloorToInt(invulnerabilityTimer * 10) % 2 == 0;

            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                sprite.enabled = true;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
