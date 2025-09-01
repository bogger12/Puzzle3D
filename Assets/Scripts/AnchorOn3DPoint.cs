using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnchorOn3DPoint : MonoBehaviour
{


    public Transform anchor;
    private Vector3 anchorPos;
    private Camera sceneCamera;
    public Vector2 offset2D;
    public bool lockToScreenBounds;

    private Vector2 defaultSize;
    public float sizeMultiplier = 1;

    RectTransform rt;
    Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        canvas = transform.GetComponentInParent<Canvas>();
        sceneCamera = canvas.worldCamera;
        defaultSize = rt.sizeDelta;
    }

    void LateUpdate()
    {
        Vector3 anchorPoint = anchor != null ? anchor.position : anchorPos;
        Vector3 screenPoint = sceneCamera.WorldToScreenPoint(anchorPoint);
        if (lockToScreenBounds) screenPoint = new Vector2(Mathf.Clamp(screenPoint.x, 0, sceneCamera.pixelWidth), Mathf.Clamp(screenPoint.y, 0, sceneCamera.pixelHeight));

        Vector2 uiPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,            // the root canvas RectTransform
            screenPoint,                 // from step 2
            sceneCamera,                       // the camera rendering this UI
            out uiPoint
        );

        rt.anchoredPosition = uiPoint + offset2D * canvas.scaleFactor;

        rt.sizeDelta = defaultSize * sizeMultiplier;
        Debug.Log("Viewport Point " + screenPoint);
    }

    public void SetAnchorPos(Vector3 pos)
    {
        anchorPos = pos;
    }
}
