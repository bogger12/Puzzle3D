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
    protected Rigidbody _holdingBody;
    protected PlayerThrow playerThrow;

    public Rigidbody HoldingBody
    {
        get => _holdingBody;
        set
        {
            playerThrow = value != null ? value.GetComponent<PlayerThrow>() : null;
            _holdingBody = value;
        }
    }

    public bool allowRotationCarryOver = true;
    public bool freezeRotationDuringCarry = false;
    public bool matchPlayerRotation = false;
    public bool lockItemToPlayer = false;
    public bool customHeldRotation = false;
    public Quaternion heldRotation = Quaternion.identity;
    protected ConfigurableJoint holdJoint = null;
    public Rigidbody rb { get; protected set; }

    public bool canBeHeld = true;
    public float followSpeed = 1;

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
        else if (heldStatus == HoldableStatus.Held)
        {
            // lerp pos to holder
            Vector3 moveTo = Vector3.Lerp(rb.position, holdPoint.position, 20 * Time.fixedDeltaTime);
            rb.MovePosition(moveTo);
            if (matchPlayerRotation) rb.MoveRotation(HoldingBody.rotation);
            if (customHeldRotation) rb.MoveRotation(HoldingBody.rotation * heldRotation);

        }
    }

    public virtual void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        heldStatus = HoldableStatus.Held;
        this.HoldingBody = holdingBody;

        rb.position = holdAnchor.position;
        holdPoint = holdAnchor;

        // Store original values
        originalFreezeRotation = rb.freezeRotation;
        rb.freezeRotation = freezeRotationDuringCarry;

        originalRotation = rb.rotation;
        if (customHeldRotation) rb.rotation = holdingBody.rotation * heldRotation;
        if (matchPlayerRotation) rb.rotation = holdingBody.rotation;
        rb.transform.rotation = rb.rotation;

        originalMass = rb.mass;
        rb.mass = 0f; //TODO: find alternative

        SetBodyDocile(holdingBody, true);
        controlUI = playerThrow.controlUI;
        // controlUI.SetTargetAndTexts(this, currentPlayerInputs.GetButtonText(currentPlayerInputs.holdThrow), GetControlHint());
        controlUI.SetHoldableTarget(null);
        AssignStaticHints(true);
        gameObject.layer = LayerMask.NameToLayer("Held");
    }

    public virtual void OnThrow(float physicsIgnoreTime)
    {
        SetBodyDocile(HoldingBody, false, physicsIgnoreTime);
        heldStatus = HoldableStatus.NotHeld;
        RemoveFromHoldingBody();
        // Destroy(holdJoint);

        rb.freezeRotation = originalFreezeRotation;
        if (!allowRotationCarryOver) rb.rotation = originalRotation;

        rb.mass = originalMass;

        controlUI.SetHoldableTarget(null);
        controlUI = null;
        AssignStaticHints(false); // needs holdingbody (playerthrow)
        HoldingBody = null;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
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
        rb.isKinematic = docile;

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
        PlayerThrow playerThrow = HoldingBody.GetComponent<PlayerThrow>();
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

    public virtual void AssignStaticHints(bool display)
    {
        ControlHintsManager controlHintsManager = playerThrow.controlHintsManager;
        controlHintsManager.ResetHints();
        if (display)
        {
            PlayerInputs playerInputs = playerThrow.playerInputs;
            controlHintsManager.AssignHint(playerInputs.GetButtonText(playerInputs.holdThrow), "Drop", false);
            controlHintsManager.AssignHint(playerInputs.GetButtonText(playerInputs.holdThrow), "Throw", true);
        }
    }
}
