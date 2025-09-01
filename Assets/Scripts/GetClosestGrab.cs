using UnityEngine;

public class GetClosestGrab : MonoBehaviour
{
    public Transform grabPoints;
    public AnchorOn3DPoint anchorObject;

    [Range(0,1)]
    public float moveSpeed;


    private Vector3 currentPos;
    private Vector3 targetPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grabPoints.childCount == 0) return;
        Transform closestPoint = grabPoints.GetChild(0);

        Transform[] points = grabPoints.GetComponentsInChildren<Transform>();
        foreach (Transform point in points)
        {
            if (Vector3.Distance(transform.position, point.position) < Vector3.Distance(transform.position, closestPoint.position))
            {
                closestPoint = point;
            }
        }

        if (closestPoint.position != targetPos) targetPos = closestPoint.position;
        currentPos = Vector3.Lerp(currentPos, targetPos, moveSpeed);
        anchorObject.SetAnchorPos(currentPos);
        // anchorObject.anchor = closestPoint;
    }
}
