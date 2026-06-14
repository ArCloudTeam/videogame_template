using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Patrulla")]
    public float patrolSpeed = 2f;
    public float leftBound = -5f;
    public float rightBound = -2f;

    [Header("Daño")]
    public int damage = 1;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private int direction = 1;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        Vector2 targetPos = rb.position + Vector2.right * direction * patrolSpeed * Time.fixedDeltaTime;

        if (targetPos.x >= rightBound) direction = -1;
        else if (targetPos.x <= leftBound) direction = 1;

        targetPos = rb.position + Vector2.right * direction * patrolSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);

        if (sprite != null) sprite.flipX = direction < 0;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
