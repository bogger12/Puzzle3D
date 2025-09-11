using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnchorOn3DPoint : MonoBehaviour
{


    public Transform anchor;
    private Vector3 anchorPos;
    private Camera sceneCamera;
    public Vector2 offset2D;
    public bool lockToScreenBounds;

    protected Vector3 defaultScale;

    RectTransform rt;
    Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rt = GetComponent<RectTransform>();
        canvas = transform.GetComponentInParent<Canvas>();
        sceneCamera = canvas.worldCamera;
        defaultScale = rt.localScale;
    }

    protected virtual void LateUpdate()
    {
        if (anchor is RectTransform a)
        {
            if ((RectTransform)transform.parent == a) rt.anchoredPosition = Vector3.zero;
            else rt.anchoredPosition = a.anchoredPosition;
            return;
        }
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

        // rt.localScale = defaultScale * sizeMultiplier;
        // Debug.Log("Viewport Point " + screenPoint);
    }

    public virtual void SetAnchorPos(Vector3 pos)
    {
        anchorPos = pos;
    }
}