using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
public class PlayerThrow : MonoBehaviour
{

    // Input Component
    private PlayerInputs playerInputs;

    public Rigidbody heldBody = null;
    public Transform holdAnchor;
    public Transform dropAnchor;
    public float dropTime = 0.2f;

    public Vector3 throwDirectionForward = new Vector3(0, 0.5f, 0.5f);
    public Vector2 throwForceRange = new Vector2(0, 5);
    public float minThrowSeconds = 0.1f;
    public float maxThrowSeconds = 2;
    public float waitBeforeReenablePhysicsSeconds = 0.2f;
    [Range(0, 1)]
    public float parentBodyVelocityAddFactor = 0;


    [Header("Circlecast")]
    public LayerMask castLayerMask;

    [Header("UI")]
    public Image UIThrowProgressImage;
    public ControlAnchorOnHoldable controlUI;

    private Rigidbody rb;

    // Frame carryover values
    private List<Collider> lastColliders = new List<Collider>();
    private float timeSincePressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInputs = GetComponent<PlayerInputs>();

#if UNITY_EDITOR
        // Set UIThrowProgressImage Material to make sure we aren't modifying the asset
        UIThrowProgressImage.material = new Material(UIThrowProgressImage.material);
#endif
    }

    // Update is called once per frame
    void Update()
    {

        // Get near items and nearest
        List<Collider> collidersList = OutlineNearItems();
        Collider nearest = collidersList.Count > 0 ? GetNearestCollider(collidersList) : null;


        float throwProgress = Mathf.Clamp01(timeSincePressed / maxThrowSeconds);

        // Checking key up first so timeSincePressed = 0 not done before keydown
        if (playerInputs.holdThrow.WasReleasedThisFrame())
        {
            if (nearest != null && heldBody == null && nearest.attachedRigidbody != null)
            {
                // Make body docile
                if (nearest.attachedRigidbody.transform.TryGetComponent<Holdable>(out Holdable holdable) && holdable.canBeHeld)
                {
                    heldBody = nearest.attachedRigidbody;
                    if (holdable.heldStatus == HoldableStatus.Held)
                    {
                        holdable.OnThrow(0f);
                    }
                    if (holdable.heldStatus == HoldableStatus.NotHeld)
                    {
                        holdable.HeldBy(rb, holdAnchor);
                        // if (holdable.heldStatus == HoldableStatus.NotHeld) heldBody = null;
                    }
                }
            }
            else if (heldBody != null && timeSincePressed >= minThrowSeconds)
            {
                Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
                float throwForce = Mathf.Lerp(throwForceRange.x, throwForceRange.y, throwProgress);
                Debug.Log("Throwing Item with force:" + throwForce);
                // Reset body vars before throw
                if (heldBody.transform.TryGetComponent<Holdable>(out Holdable holdable) && holdable.heldStatus == HoldableStatus.Held)
                {
                    Rigidbody heldBody = this.heldBody; // heldBody is nullified in OnThrow
                    holdable.OnThrow(waitBeforeReenablePhysicsSeconds);
                    heldBody.linearVelocity = rb.linearVelocity * parentBodyVelocityAddFactor;
                    heldBody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
                }
                timeSincePressed = 0;
            }
            else if (heldBody != null && timeSincePressed < minThrowSeconds)
            {
                if (heldBody.transform.TryGetComponent<Holdable>(out Holdable holdable))
                {
                    if (holdable.heldStatus == HoldableStatus.Held)
                    {
                        holdable.OnInteractDrop(holdAnchor, dropAnchor, dropTime);
                    }
                }
                timeSincePressed = 0;
            }
        }

        if (playerInputs.holdThrow.IsPressed())
        {
            // Setting the throw strength
            Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
            Debug.DrawRay(rb.position, throwDirection * timeSincePressed, Color.red);
            timeSincePressed += Time.deltaTime;
            // Update Throw Progress in Material
            if (heldBody != null)
            {
                float progress = timeSincePressed / maxThrowSeconds;
                UIThrowProgressImage.enabled = true;
                UIThrowProgressImage.material.SetFloat("_Progress", progress);
            }
        }
        else
        {
            UIThrowProgressImage.enabled = false;
            timeSincePressed = 0;
        }
    }


    List<Collider> OutlineNearItems()
    {
        // Do phyisic cast to find items within radius
        float castRadius = 2f;
        Collider[] colliders = Physics.OverlapSphere(rb.position, castRadius, castLayerMask);
        List<Collider> collidersList = colliders.ToList<Collider>();
        Holdable closest = null;
        foreach (Collider c in colliders)
        {
            if (c.gameObject.TryGetComponent<Outline>(out Outline outlineScript) && c.gameObject.TryGetComponent<Holdable>(out Holdable holdable))
            {
                if (closest == null || Vector3.Distance(holdable.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = holdable;
                }
                outlineScript.Enabled = holdable.canBeHeld;
                if (!holdable.canBeHeld) collidersList.Remove(c);
            }
        }
        controlUI.SetHoldableTarget(closest); // null or real
        var collidersExitedRange = lastColliders.Except(collidersList); // Any Colliders left over from last frame that aren't within range this frame
        lastColliders = collidersList;
        foreach (Collider c in collidersExitedRange)
        {
            if (c!=null && c.gameObject.TryGetComponent<Outline>(out Outline outlineScript))
            {
                outlineScript.Enabled = false;
            }
        }
        return collidersList;
    }

    Collider GetNearestCollider(List<Collider> collidersList)
    {
        Collider closest = collidersList[0];
        float minDistance = 0;
        foreach (Collider c in collidersList)
        {
            float distance = Vector3.Distance(rb.position, c.gameObject.transform.position);
            if (minDistance == 0 || distance < minDistance)
            {
                minDistance = distance;
                closest = c;
            }
        }
        return closest;
    }
}
