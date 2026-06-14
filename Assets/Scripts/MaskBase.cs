using UnityEngine;

public abstract class MaskBase : MonoBehaviour
{
    public abstract bool CanActivate();
    public abstract void Activate();
}
