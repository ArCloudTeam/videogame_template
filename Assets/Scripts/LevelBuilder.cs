using UnityEngine;
using UnityEngine.InputSystem;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        if (GameObject.Find("LevelManager") == null)
        {
            var go = new GameObject("LevelManager");
            go.AddComponent<LevelBuilder>();
        }
    }
}

public class LevelBuilder : MonoBehaviour
{
    private CameraController camCtrl;

    void Awake()
    {
        if (Camera.main == null) return;
        if (GameObject.Find("Player") != null) return;

        BuildLevel();
    }

    void BuildLevel()
    {
        SetupCamera();
        Transform player = CreatePlayer();
        if (camCtrl != null) camCtrl.target = player;
        CreateGround();
        CreatePlatforms();
        CreateEnemies();
    }

    void SetupCamera()
    {
        Camera cam = Camera.main;
        cam.orthographic = true;
        cam.orthographicSize = 5;
        cam.backgroundColor = new Color(0.15f, 0.15f, 0.2f);

        camCtrl = cam.gameObject.AddComponent<CameraController>();
    }

    Transform CreatePlayer()
    {
        Vector3 spawnPos = new Vector3(-6f, -2f, 0f);

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = spawnPos;

        SpriteRenderer sprite = player.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(32, 32, Color.cyan);
        sprite.sortingLayerName = "Default";

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.8f, 1f);

        GameObject check = new GameObject("GroundCheck");
        check.transform.parent = player.transform;
        check.transform.localPosition = new Vector3(0f, -0.6f, 0f);

        PlayerController controller = player.AddComponent<PlayerController>();
        controller.groundCheck = check.transform;

        player.AddComponent<PlayerHealth>();

        CreateAttackHitbox(player);

        PlayerInput playerInput = player.AddComponent<PlayerInput>();
#if UNITY_EDITOR
        playerInput.actions = UnityEditor.AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");
#endif
        playerInput.defaultActionMap = "Player";
        playerInput.notificationBehavior = PlayerNotifications.SendMessages;

        return player.transform;
    }

    void CreateGround()
    {
        GameObject ground = new GameObject("Ground");
        ground.transform.position = new Vector3(0f, -4.5f, 0f);

        SpriteRenderer sprite = ground.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(32, 32, Color.gray);
        sprite.drawMode = SpriteDrawMode.Tiled;
        sprite.size = new Vector2(30f, 1f);

        BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(30f, 1f);
        collider.offset = new Vector2(0f, 0f);
    }

    void CreatePlatforms()
    {
        CreateSinglePlatform(new Vector3(3f, 0f, 0f), new Vector2(3f, 0.5f), "Platform_1");
        CreateSinglePlatform(new Vector3(-2f, 2f, 0f), new Vector2(3f, 0.5f), "Platform_2");
        CreateSinglePlatform(new Vector3(6f, -1.5f, 0f), new Vector2(3f, 0.5f), "Platform_3");
        CreateSinglePlatform(new Vector3(-6f, 0.5f, 0f), new Vector2(3f, 0.5f), "Platform_4");
    }

    void CreateSinglePlatform(Vector3 position, Vector2 size, string name)
    {
        GameObject platform = new GameObject(name);
        platform.transform.position = position;

        SpriteRenderer sprite = platform.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(32, 32, new Color(0.4f, 0.4f, 0.4f));
        sprite.drawMode = SpriteDrawMode.Tiled;
        sprite.size = size;

        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
        collider.size = size;
    }

    void CreateAttackHitbox(GameObject player)
    {
        GameObject hitbox = new GameObject("AttackHitbox");
        hitbox.transform.parent = player.transform;
        hitbox.transform.localPosition = new Vector3(0.8f, 0f, 0f);

        SpriteRenderer sprite = hitbox.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(16, 16, Color.white);
        sprite.color = new Color(1f, 1f, 1f, 0.6f);
        sprite.sortingLayerName = "Default";

        BoxCollider2D collider = hitbox.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.5f, 0.6f);
        collider.isTrigger = true;

        hitbox.AddComponent<AttackHitbox>();
        hitbox.SetActive(false);

        PlayerAttack attack = player.AddComponent<PlayerAttack>();
        attack.attackHitbox = hitbox;
    }

    void CreateEnemies()
    {
        CreateEnemy(new Vector3(-3.5f, -3.5f, 0f), -5f, -2f);
        CreateEnemy(new Vector3(6.5f, -3.5f, 0f), 5f, 8f);
    }

    void CreateEnemy(Vector3 position, float leftBound, float rightBound)
    {
        GameObject enemy = new GameObject("Enemy");
        enemy.transform.position = position;

        SpriteRenderer sprite = enemy.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(24, 24, Color.red);
        sprite.sortingLayerName = "Default";

        Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        BoxCollider2D collider = enemy.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.8f, 0.9f);

        EnemyController eCtrl = enemy.AddComponent<EnemyController>();
        eCtrl.leftBound = leftBound;
        eCtrl.rightBound = rightBound;
    }

    Sprite CreateSolidSprite(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 32f, 0, SpriteMeshType.FullRect);
    }
}
