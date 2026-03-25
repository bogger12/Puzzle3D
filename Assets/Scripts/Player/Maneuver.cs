using System.Collections.Generic;
using UnityEngine;

public class Maneuever : MonoBehaviour
{

    public Camera mainCamera;

    private Vector3 currentLock = Vector3.positiveInfinity;
    private Rigidbody rb;

    public LayerMask wallDetect;

    public float moveSpeed = 10;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && float.IsInfinity(currentLock.magnitude)) currentLock = ClosestHitPoint();
        else if (Input.GetKeyUp(KeyCode.Mouse0)) currentLock = Vector3.positiveInfinity;

        Debug.Log(currentLock);
        Debug.DrawRay(transform.position, mainCamera.transform.rotation * Vector3.forward * 10, Color.cyan);
        Debug.DrawLine(transform.position, currentLock, Color.green);
        if (!float.IsInfinity(currentLock.magnitude)) PullTowards(currentLock);
    }


    Vector3 ClosestHitPoint()
    {
        List<RaycastHit> hits = new(Physics.RaycastAll(mainCamera.transform.position, mainCamera.transform.rotation * Vector3.forward, 100, wallDetect));
        Vector3 closest = Vector3.positiveInfinity;
        foreach (var hit in hits)
        {
            if (Vector3.Distance(transform.position, hit.point) < Vector3.Distance(transform.position, closest))
                closest = hit.point;
        }
        return closest;
    }

    void PullTowards(Vector3 pos)
    {
        rb.MovePosition(Vector3.Lerp(rb.position, pos, Time.deltaTime * moveSpeed));
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(currentLock, Vector2.one);
    }
}