using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnchorOn3DPoint : MonoBehaviour
{


    public Transform anchor;
    private Vector3 anchorPos;
    public Camera sceneCamera;
    public Vector2 offset2D;
    public bool lockToScreenBounds;

    RectTransform rt;
    Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        canvas = transform.GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        Vector3 anchorPoint = anchor != null ? anchor.position : anchorPos;
        Vector2 screenPoint = sceneCamera.WorldToScreenPoint(anchorPoint);
        if (lockToScreenBounds) screenPoint = new Vector2(Mathf.Clamp(screenPoint.x, 0, sceneCamera.pixelWidth), Mathf.Clamp(screenPoint.y, 0, sceneCamera.pixelHeight));
        rt.position = screenPoint + offset2D * canvas.scaleFactor;
    }

    public void SetAnchorPos(Vector3 pos)
    {
        anchorPos = pos;
    }
}
