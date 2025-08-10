using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnchorOn3DPoint : MonoBehaviour
{


    public Transform anchor;
    public Camera sceneCamera;
    public Vector2 offset2D;

    RectTransform rt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPoint = sceneCamera.WorldToScreenPoint(anchor.position);
        rt.position = screenPoint + offset2D;
    }
}
