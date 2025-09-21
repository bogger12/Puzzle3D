using UnityEngine;

public class ParentToPlatform : MonoBehaviour
{
    private Rigidbody rb;
    Transform currentPlatform = default;
    public float detectionDistance = 1.1f;
    public LayerMask platformLayer;


    public Vector3 platformLastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform onPlatform = OnPlatform();

        if (onPlatform == currentPlatform && onPlatform != null) {
            Vector3 platformDeltaPos = onPlatform.position - platformLastPos;
            rb.MovePosition(rb.position+platformDeltaPos);
            platformLastPos = onPlatform.position;
        } else if (onPlatform != null) {
            platformLastPos = onPlatform.position;
        }
        currentPlatform = onPlatform;
        
    }

    private Transform OnPlatform()
    {
        Vector3 castFrom = transform.position + transform.up * 1f;
        bool didHit = Physics.SphereCast(castFrom, 0.5f, -transform.up, out RaycastHit hit, detectionDistance + 0.5f, platformLayer);
        return hit.transform;
    }
    
}
