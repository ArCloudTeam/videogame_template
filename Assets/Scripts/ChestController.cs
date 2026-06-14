using UnityEngine;
using UnityEngine.InputSystem;

public class ChestController : MonoBehaviour
{
    public GameObject maskIcon;

    private SpriteRenderer sprite;
    private bool isOpened;
    private bool playerInRange;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (maskIcon != null) maskIcon.SetActive(false);
    }

    void Update()
    {
        if (!isOpened && playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
            Open();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Open()
    {
        isOpened = true;
        sprite.color = new Color(0.5f, 0.3f, 0.1f);

        if (maskIcon != null)
        {
            maskIcon.SetActive(true);
            Invoke(nameof(HideMaskIcon), 1.5f);
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<MaskController>();
            if (controller != null)
            {
                controller.UnlockMask();
                var feedback = controller.GetComponent<SpriteRenderer>();
                if (feedback != null)
                {
                    Color c = feedback.color;
                    c.a = 0.5f;
                    feedback.color = c;
                    Invoke(nameof(RestorePlayerAlpha), 0.3f);
                }
            }
        }

        Destroy(GetComponent<Collider2D>());
    }

    void RestorePlayerAlpha()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }
    }

    void HideMaskIcon()
    {
        if (maskIcon != null)
            maskIcon.SetActive(false);
    }
}
