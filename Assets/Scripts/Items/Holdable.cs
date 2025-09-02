using UnityEngine;

public abstract class Holdable : MonoBehaviour
{
    public bool isHeld;
    public virtual void SetIsHeld(bool held)
    {
        isHeld = held;
    }
}
