using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject attackHitbox;
    public float attackDuration = 0.15f;
    public float attackCooldown = 0.3f;

    private float cooldownTimer;

    void Update()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && cooldownTimer <= 0)
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        cooldownTimer = attackCooldown;

        if (attackHitbox != null)
        {
            float dir = GetComponent<SpriteRenderer>() != null && GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
            attackHitbox.transform.localPosition = new Vector3(dir * 0.8f, 0f, 0f);
            attackHitbox.SetActive(true);
            Invoke(nameof(DisableHitbox), attackDuration);
        }
    }

    void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
