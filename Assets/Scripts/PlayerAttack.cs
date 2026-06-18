using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject attackHitbox;
    public float attackDuration = 0.15f;
    public float attackCooldown = 0.3f;

    private float cooldownTimer;

    [Header("Disparo")]
    public float shootCooldown = 0.4f;
    public float bulletSpeed = 15f;

    private float shootCooldownTimer;

    void Update()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        if (shootCooldownTimer > 0) shootCooldownTimer -= Time.deltaTime;
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

    public void OnShoot(InputValue value)
    {
        if (value.isPressed && shootCooldownTimer <= 0)
            Shoot();
    }

    void Shoot()
    {
        shootCooldownTimer = shootCooldown;

        float dir = GetComponent<SpriteRenderer>() != null && GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
        Vector3 spawnPos = transform.position + new Vector3(dir * 0.8f, 0f, 0f);

        GameObject bullet = new GameObject("Bullet");
        bullet.transform.position = spawnPos;

        SpriteRenderer sr = bullet.AddComponent<SpriteRenderer>();
        sr.sprite = CreateBulletSprite();
        sr.sortingLayerName = "Default";

        Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        BoxCollider2D bc = bullet.AddComponent<BoxCollider2D>();
        bc.size = new Vector2(0.5f, 0.5f);
        bc.isTrigger = true;

        Bullet bulletScript = bullet.AddComponent<Bullet>();
        bulletScript.Initialize(new Vector2(dir, 0f), bulletSpeed);
    }

    Sprite CreateBulletSprite()
    {
        Texture2D tex = new Texture2D(16, 16);
        Color[] pixels = new Color[256];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.yellow;
        tex.SetPixels(pixels);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 32f);
    }
}
