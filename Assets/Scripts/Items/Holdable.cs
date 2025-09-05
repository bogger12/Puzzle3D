using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
public class Holdable : MonoBehaviour
{
    public bool isHeld;
    protected Rigidbody holdingBody;

    public bool allowRotationCarryOver = true;
    public bool freezeRotationDuringCarry = false;
    public bool lockItemToPlayer = false;
    public bool customHeldRotation = false;
    public Quaternion heldRotation = Quaternion.identity;
    protected ConfigurableJoint holdJoint = null;
    protected Rigidbody rb;

    // Original Values
    private float originalMass;
    private Quaternion originalRotation;
    private bool originalFreezeRotation;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        isHeld = true;
        this.holdingBody = holdingBody;

        rb.position = holdAnchor.position;

        // Store original values
        originalFreezeRotation = rb.freezeRotation;
        rb.freezeRotation = freezeRotationDuringCarry;

        originalRotation = rb.rotation;
        if (customHeldRotation) rb.rotation = holdingBody.rotation * heldRotation;
        rb.transform.rotation = rb.rotation;

        originalMass = rb.mass;
        rb.mass = 0f; //TODO: find alternative


        // Add joint to object
        holdJoint = gameObject.AddComponent<ConfigurableJoint>();

        holdJoint.connectedBody = holdingBody; // player rigidbody
        holdJoint.autoConfigureConnectedAnchor = false;
        holdJoint.anchor = Vector3.zero;
        holdJoint.connectedAnchor = holdAnchor.localPosition;
        holdJoint.xMotion = ConfigurableJointMotion.Locked;
        holdJoint.yMotion = ConfigurableJointMotion.Locked;
        holdJoint.zMotion = ConfigurableJointMotion.Locked;
        if (lockItemToPlayer)
        {
            holdJoint.angularXMotion = ConfigurableJointMotion.Locked;
            holdJoint.angularYMotion = ConfigurableJointMotion.Locked;
            holdJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }
    }

    public virtual void OnThrow()
    {
        isHeld = false;
        holdingBody = null;
        Destroy(holdJoint);

        rb.freezeRotation = originalFreezeRotation;
        if (!allowRotationCarryOver) rb.rotation = originalRotation;

        rb.mass = originalMass;
    }


    public void SetBodyDocile(Rigidbody holdingBody, bool docile, float waitBeforeReenablePhysicsSeconds = 0)
    {
        Collider c = holdingBody.GetComponent<Collider>();
        rb.useGravity = !docile;

        if (docile)
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            Physics.IgnoreCollision(transform.GetComponent<Collider>(), c, true);
        }
        else
        {
            rb.interpolation = RigidbodyInterpolation.None;
            StartCoroutine(SetIgnoreCollisionAfter(waitBeforeReenablePhysicsSeconds, c));
        }
    }

    private IEnumerator<WaitForSeconds> SetIgnoreCollisionAfter(float duration, Collider c)
    {
        // Eventual code to execute right as the function is called
        yield return new WaitForSeconds(duration);
        // The code from here will be executed after **duration** seconds
        if (transform == null) yield break;
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), c, false);
    }

    public void RemoveFromHoldingBody()
    {
        PlayerThrow playerThrow = holdingBody.GetComponent<PlayerThrow>();
        playerThrow.heldBody = null;
    }
}
