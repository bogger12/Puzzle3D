using UnityEngine;

[RequireComponent(typeof(RemoteActivate))]
public class PressurePlate : MonoBehaviour
{
    private RemoteActivate remoteActivate;
    public bool setOnActivate = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remoteActivate = GetComponent<RemoteActivate>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        remoteActivate.SetActive(setOnActivate);
    }
    public void OnTriggerStay(Collider collider)
    {
        if (remoteActivate.activateable.active != setOnActivate) remoteActivate.SetActive(setOnActivate);
    }

    public void OnTriggerExit(Collider other)
    {
        remoteActivate.SetActive(setOnActivate);
    }
}
