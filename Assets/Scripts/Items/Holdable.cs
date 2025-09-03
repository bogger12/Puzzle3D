using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Holdable : MonoBehaviour
{
    protected bool isHeld;
    protected Rigidbody holdingBody;

    public bool allowRotationCarryOver = true;
    public bool FreezeRotationDuringCarry = false;
    public bool customHeldRotation = false;
    public Quaternion heldRotation = Quaternion.identity;

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
