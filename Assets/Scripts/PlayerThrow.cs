using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{

    public Rigidbody heldBody = null;
    public Transform holdAnchor;
    public Vector3 throwDirectionForward = new Vector3(0, 0.5f, 0.5f);
    public float throwForceMult = 1;
    public float minThrowSeconds = 2;


    [Header("Circlecast")]
    public LayerMask castLayerMask;

    private Rigidbody rb;

    // Frame carryover values
    private List<Collider> lastColliders = new List<Collider>();
    private float timeSincePressed;
    private float heldBodyOriginalMass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // Get near items and nearest
        List<Collider> collidersList = OutlineNearItems();
        Collider nearest = collidersList.Count>0 ? GetNearestCollider(collidersList) : null;


        // Checking key up first so timeSincePressed = 0 not done before keydown
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (nearest != null && heldBody == null && nearest.attachedRigidbody != null)
            {
                heldBody = nearest.attachedRigidbody;
                heldBody.position = holdAnchor.position;
                heldBody.useGravity = false;
                heldBodyOriginalMass = heldBody.mass;
                heldBody.mass = 0;
            }
            else if (heldBody != null && timeSincePressed >= minThrowSeconds)
            {
                Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
                float throwForce = throwForceMult * timeSincePressed;
                // Restore mass before throw
                heldBody.mass = heldBodyOriginalMass;
                heldBody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

                // Make body docile
                heldBody.useGravity = true;
                timeSincePressed = 0;
                heldBody = null;

            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            // Setting the throw strength
            Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
            Debug.DrawRay(rb.position, throwDirection * timeSincePressed, Color.red);
            timeSincePressed += Time.deltaTime;
        }
        else timeSincePressed = 0;


        if (heldBody != null) heldBody.position = holdAnchor.position;

    }


    List<Collider> OutlineNearItems()
    {
        // Do phyisic cast to find items within radius
        float castRadius = 2f;
        Collider[] colliders = Physics.OverlapSphere(rb.position, castRadius, castLayerMask);
        foreach (Collider c in colliders) {
            if (c.gameObject.TryGetComponent<Outline>(out Outline outlineScript)) {
                outlineScript.Enabled = true;
            }
        }
        List<Collider> collidersList = colliders.ToList<Collider>();
        var collidersExitedRange = lastColliders.Except(collidersList); // Any Colliders left over from last frame that aren't within range this frame
        lastColliders = collidersList;
        foreach (Collider c in collidersExitedRange) {
            if (c.gameObject.TryGetComponent<Outline>(out Outline outlineScript)) {
                outlineScript.Enabled = false;
            }
        }
        return collidersList;
    }

    Collider GetNearestCollider(List<Collider> collidersList)
    {
        Collider closest = collidersList[0];
        float minDistance = 0;
        foreach (Collider c in collidersList) {
            float distance = Vector3.Distance(rb.position, c.gameObject.transform.position);
            if (minDistance == 0 || distance < minDistance) {
                minDistance = distance;
                closest = c;
            }
        }
        return closest;
    }
}
