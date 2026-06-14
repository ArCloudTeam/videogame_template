using UnityEngine;

public class GhostMask : MaskBase
{
    [Header("Duración")]
    public float duration = 3f;
    public float cooldown = 5f;

    private bool isActive;
    private float activeTimer;
    private float cooldownTimer;
    private SpriteRenderer sprite;
    private PlayerHealth playerHealth;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public override bool CanActivate()
    {
        return !isActive && cooldownTimer <= 0f;
    }

    public override void Activate()
    {
        isActive = true;
        activeTimer = duration;

        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = 0.3f;
            sprite.color = c;
        }

        if (playerHealth != null)
            playerHealth.SetInvulnerable(true);

        EnemyController[] allEnemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
            enemy.SetAlert(false);
    }

    void Update()
    {
        if (isActive)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0f) Deactivate();
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void Deactivate()
    {
        isActive = false;
        cooldownTimer = cooldown;

        if (sprite != null)
        {
            Color c = sprite.color;
            c.a = 1f;
            sprite.color = c;
        }

        if (playerHealth != null)
            playerHealth.SetInvulnerable(false);

        EnemyController[] allEnemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
        {
            if (enemy != null) enemy.SetAlert(true);
        }
    }
}
