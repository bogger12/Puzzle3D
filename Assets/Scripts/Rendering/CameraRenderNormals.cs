using UnityEngine;

[ExecuteAlways] // Works in Edit mode too
public class CameraRenderNormals : MonoBehaviour
{
    void OnEnable()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode |= DepthTextureMode.DepthNormals;
    }
}
