using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum HoldableStatus
{
    NotHeld,
    Held,
    Dropping
}

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Holdable : MonoBehaviour
{
    public HoldableStatus heldStatus;
    protected Rigidbody holdingBody;

    public bool allowRotationCarryOver = true;
    public bool freezeRotationDuringCarry = false;
    public bool lockItemToPlayer = false;
    public bool customHeldRotation = false;
    public Quaternion heldRotation = Quaternion.identity;
    protected ConfigurableJoint holdJoint = null;
    public Rigidbody rb { get; protected set; }

    public bool canBeHeld = true;

    // Original Values
    private float originalMass;
    private Quaternion originalRotation;
    private bool originalFreezeRotation;


    // Dropping stuff

    private float currentDropTime = 0.2f;
    private float dropTimer = 0f;
    private Transform holdPoint;
    private Transform dropPoint;

    private ControlAnchorOnPoint controlUI = null;
    private PlayerInputs currentPlayerInputs;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        // Do dropping
        if (heldStatus == HoldableStatus.Dropping)
        {
            dropTimer += Time.fixedDeltaTime;
            float t = dropTimer / currentDropTime;
            Vector3 moveTo = Vector3.Slerp(holdPoint.position, dropPoint.position, t);
            if (t >= 1)
            {
                moveTo = dropPoint.position;
                heldStatus = HoldableStatus.NotHeld;
                dropTimer = 0;
            }
            rb.MovePosition(moveTo);
        }
    }

    public virtual void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        heldStatus = HoldableStatus.Held;
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

        SetBodyDocile(holdingBody, true);
        PlayerThrow playerThrow = holdingBody.GetComponent<PlayerThrow>();
        currentPlayerInputs = playerThrow.GetComponent<PlayerInputs>();
        controlUI = playerThrow.controlUI;
        // controlUI.SetTargetAndTexts(this, currentPlayerInputs.GetButtonText(currentPlayerInputs.holdThrow), GetControlHint());
        controlUI.SetHoldableTarget(null);
    }

    public virtual void OnThrow(float physicsIgnoreTime)
    {
        SetBodyDocile(holdingBody, false, physicsIgnoreTime);
        heldStatus = HoldableStatus.NotHeld;
        RemoveFromHoldingBody();
        holdingBody = null;
        Destroy(holdJoint);

        rb.freezeRotation = originalFreezeRotation;
        if (!allowRotationCarryOver) rb.rotation = originalRotation;

        rb.mass = originalMass;

        controlUI.SetHoldableTarget(null);
        controlUI = null;
    }

    public virtual void OnInteractDrop(Transform holdPoint, Transform dropPoint, float dropTime)
    {
        OnThrow(dropTime);
        if (holdPoint && dropPoint)
        {
            heldStatus = HoldableStatus.Dropping;
            this.holdPoint = holdPoint;
            this.dropPoint = dropPoint;
            this.currentDropTime = dropTime;
        }
    }


    protected void SetBodyDocile(Rigidbody holdingBody, bool docile, float waitBeforeReenablePhysicsSeconds = 0)
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

    protected void RemoveFromHoldingBody()
    {
        PlayerThrow playerThrow = holdingBody.GetComponent<PlayerThrow>();
        playerThrow.HeldBody = null;
    }

    public string GetControlHint()
    {
        return heldStatus switch
        {
            HoldableStatus.NotHeld => "Pick Up",
            HoldableStatus.Dropping => "You shouldn't see this",
            HoldableStatus.Held => "Throw",
            _ => null,
        };
    }

    public bool GetIsLongPress()
    {
        return heldStatus switch
        {
            HoldableStatus.NotHeld => false,
            HoldableStatus.Dropping => false,
            HoldableStatus.Held => true,
            _ => false,
        };
    }
}
