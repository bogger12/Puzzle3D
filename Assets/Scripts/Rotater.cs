using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineSplineDolly))]
public class Rotater : MonoBehaviour
{
    private CinemachineSplineDolly dolly;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dolly = GetComponent<CinemachineSplineDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        // dolly.CameraPosition = (Time.time * speed) % 1;
        dolly.CameraPosition = (Time.time * speed);
    }
}
