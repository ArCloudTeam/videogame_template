using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null) enemy.Die();
    }
}
