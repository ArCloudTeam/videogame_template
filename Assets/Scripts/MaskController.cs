using UnityEngine;
using UnityEngine.InputSystem;

public class MaskController : MonoBehaviour
{
    private MaskBase currentMask;
    private bool hasMask;

    public void UnlockMask()
    {
        if (hasMask) return;
        hasMask = true;
        currentMask = gameObject.AddComponent<GhostMask>();
    }

    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && hasMask && currentMask != null && currentMask.CanActivate())
            currentMask.Activate();
    }
}
