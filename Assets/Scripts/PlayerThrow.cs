using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{

    public Rigidbody heldBody = null;
    
    public Transform holdAnchor;
    public Vector3 throwDirectionForward = new Vector3(0,0.5f,0.5f);
    public float throwForceMult = 1;
    public float minThrowSeconds = 2;

    private Rigidbody rb;

    [Header("Circlecast")]

    public LayerMask castLayerMask;


    private float timeSincePressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checking key up first so timeSincePressed = 0 not done before keydown
        if (Input.GetKeyUp(KeyCode.E) && timeSincePressed >= minThrowSeconds && heldBody!=null)
        {
            Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
            float throwForce = throwForceMult * timeSincePressed;
            heldBody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            // Reset Body
            heldBody.useGravity = true;
            heldBody = null;
            timeSincePressed = 0;
        }

        if (Input.GetKey(KeyCode.E))
        {
            Vector3 throwDirection = Vector3.Normalize(rb.rotation * throwDirectionForward);
            Debug.DrawRay(rb.position, throwDirection * timeSincePressed, Color.red);
            // Debug.Log("Throw preseed");
            timeSincePressed += Time.deltaTime;
        }
        else timeSincePressed = 0;


        if (heldBody != null) heldBody.position = holdAnchor.position;
        
        // Do phyisic cast to find nearest item
        float castRadius = 2f;
        Collider[] colliders = Physics.OverlapSphere(rb.position, castRadius, castLayerMask);
        foreach (Collider c in colliders)
        {
            Debug.Log(c.gameObject.name);
            Outline outlineScript = c.gameObject.GetComponent<Outline>();
            outlineScript.Enabled = true;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (colliders.Length == 0) return;
            heldBody = colliders[0].attachedRigidbody;
            if (heldBody == null) return;
            heldBody.position = holdAnchor.position;
            heldBody.useGravity = false;
        }
    }
    
}
