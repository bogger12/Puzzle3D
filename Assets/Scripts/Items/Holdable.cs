using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
public class Holdable : MonoBehaviour
{
    protected bool isHeld;
    protected Rigidbody holdingBody;

    public bool allowRotationCarryOver = true;
    public bool freezeRotationDuringCarry = false;
    public bool customHeldRotation = false;
    public Quaternion heldRotation = Quaternion.identity;
    protected ConfigurableJoint holdJoint = null;
    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        isHeld = true;
        this.holdingBody = holdingBody;

        Debug.Log(rb.name);
        Debug.Log(holdAnchor.name);
        rb.position = holdAnchor.position;

        holdJoint = gameObject.AddComponent<ConfigurableJoint>();

        holdJoint.connectedBody = holdingBody; // player rigidbody
        holdJoint.autoConfigureConnectedAnchor = false;
        holdJoint.anchor = Vector3.zero;
        holdJoint.connectedAnchor = holdAnchor.localPosition;
        holdJoint.xMotion = ConfigurableJointMotion.Locked;
        holdJoint.yMotion = ConfigurableJointMotion.Locked;
        holdJoint.zMotion = ConfigurableJointMotion.Locked;
        if (!freezeRotationDuringCarry)
        {
            holdJoint.angularXMotion = ConfigurableJointMotion.Locked;
            holdJoint.angularYMotion = ConfigurableJointMotion.Locked;
            holdJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }
    }

    public virtual void NotHeld()
    {
        isHeld = false;
        holdingBody = null;
        Destroy(holdJoint);
    }
}
