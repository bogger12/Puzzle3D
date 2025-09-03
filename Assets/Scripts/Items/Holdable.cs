using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Holdable : MonoBehaviour
{
    protected bool isHeld;
    protected Rigidbody holdingBody;
    public virtual void HeldBy(Rigidbody holdingBody)
    {
        isHeld = true;
        this.holdingBody = holdingBody;
    }

    public virtual void NotHeld()
    {
        isHeld = false;
        holdingBody = null;
    }
}
