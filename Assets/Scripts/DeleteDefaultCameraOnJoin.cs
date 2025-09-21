using System.Linq;
using UnityEngine;

public class DeleteDefaultCameraOnJoin : MonoBehaviour
{
    private Camera defaultCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultCamera = FindObjectsByType<Camera>(FindObjectsSortMode.None).FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerJoined()
    {
        if (defaultCamera != null) Destroy(defaultCamera.gameObject);
    }
}
