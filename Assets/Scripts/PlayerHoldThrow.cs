using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerThrow : MonoBehaviour
{

    public Rigidbody heldBody = null;
    public Transform holdAnchor;
    public Vector3 throwDirectionForward = new Vector3(0, 0.5f, 0.5f);
    public float throwForceMult = 1;
    public float minThrowSeconds = 0.1f;
    public float maxThrowSeconds = 2;
    public float waitBeforeReenablePhysicsSeconds = 0.2f;
    [Range(0, 1)]
    public float parentBodyVelocityAddFactor = 0;


    [Header("Circlecast")]
    public LayerMask castLayerMask;

    [Header("Throw Progress UI")]
    public Image UIThrowProgressImage;

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
        Collider nearest = collidersList.Count > 0 ? GetNearestCollider(collidersList) : null;


        // Checking key up first so timeSincePressed = 0 not done before keydown
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (nearest != null && heldBody == null && nearest.attachedRigidbody != null)
            {
                heldBody = nearest.attachedRigidbody;
                // Make body docile
                heldBody.position = holdAnchor.position;
                SetBodyDocile(heldBody, true);
                heldBody.rotation = Quaternion.Euler(40f, 0f, 0);
            }
            else if (heldBody != null && timeSincePressed >= minThrowSeconds)
            {
                Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
                float throwForce = throwForceMult * timeSincePressed;
                // Reset body vars before throw
                SetBodyDocile(heldBody, false);
                heldBody.linearVelocity = rb.linearVelocity * parentBodyVelocityAddFactor;
                heldBody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

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


        if (heldBody != null) {
            heldBody.position = holdAnchor.position;
            if (Vector3.Magnitude(heldBody.angularVelocity) < 0.1f)
            {
                heldBody.AddTorque((Vector3.up) * 1f);
            }
        }

    }


    List<Collider> OutlineNearItems()
    {
        // Do phyisic cast to find items within radius
        float castRadius = 2f;
        Collider[] colliders = Physics.OverlapSphere(rb.position, castRadius, castLayerMask);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.TryGetComponent<Outline>(out Outline outlineScript))
            {
                outlineScript.Enabled = true;
            }
        }
        List<Collider> collidersList = colliders.ToList<Collider>();
        var collidersExitedRange = lastColliders.Except(collidersList); // Any Colliders left over from last frame that aren't within range this frame
        lastColliders = collidersList;
        foreach (Collider c in collidersExitedRange)
        {
            if (c.gameObject.TryGetComponent<Outline>(out Outline outlineScript))
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

    void SetBodyDocile(Rigidbody heldBody, bool docile)
    {
        Collider c = heldBody.GetComponent<Collider>();
        
        heldBody.useGravity = !docile;

        if (docile) {
            heldBodyOriginalMass = heldBody.mass;
            Physics.IgnoreCollision(transform.GetComponent<Collider>(), c, true);
            Debug.Log("Iignoring physics");
        }
        else {
            heldBody.mass = heldBodyOriginalMass;
            StartCoroutine(SetIgnoreCollisionAfter(waitBeforeReenablePhysicsSeconds, c)); 
        }
    }

    private IEnumerator<WaitForSeconds> SetIgnoreCollisionAfter(float duration, Collider c)
    {
        // Eventual code to execute right as the function is called
        yield return new WaitForSeconds(duration);
        // The code from here will be executed after **duration** seconds
        Physics.IgnoreCollision(transform.GetComponent<Collider>(), c, false);
        Debug.Log("Unignoring physics");
    }
}
