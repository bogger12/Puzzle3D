// using UnityEngine;

// public class GetClosestGrab : MonoBehaviour
// {
//     public Transform grabPoints;
//     public AnchorOn3DPoint anchorObject;

//     [Range(0,1)]
//     public float moveSpeed;
    
//     [Range(0,1)]
//     public float yInfluence;

//     public AnimationCurve sizeVsDistance;


//     private Transform playerCameraSpace;
//     private Vector3 currentPos;
//     private Vector3 targetPos;



//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         playerCameraSpace = transform.parent.GetComponent<PlayerMovement>().playerInputSpace;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (grabPoints.childCount == 0) return;
//         Transform closestPoint = grabPoints.GetChild(0);
//         for (int i = 1; i < grabPoints.childCount; i++)
//         {
//             Transform point = grabPoints.GetChild(i);

//             Vector3 cameraToPoint = point.position - playerCameraSpace.position;
//             cameraToPoint.y *= yInfluence;
//             Vector3 cameraToClosestPoint = closestPoint.position - playerCameraSpace.position;
//             cameraToClosestPoint.y *= yInfluence;

//             if (Vector3.Dot(playerCameraSpace.forward, Vector3.Normalize(cameraToPoint)) > Vector3.Dot(playerCameraSpace.forward, Vector3.Normalize(cameraToClosestPoint)))
//             {
//                 closestPoint = point;
//             }
//         }

//         if (closestPoint.position != targetPos) targetPos = closestPoint.position;
//         currentPos = Vector3.Lerp(currentPos, targetPos, moveSpeed);
//         anchorObject.SetAnchorPos(currentPos);
//         anchorObject.sizeMultiplier = sizeVsDistance.Evaluate(Vector3.Distance(playerCameraSpace.position, currentPos));
//     }
// }
