using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RemoteActivate))]
public class PressurePlate : MonoBehaviour
{
    public bool setValue = true;

    private bool active = false;

    Dictionary<Collider, bool> withinTrigger = new();

    private RemoteActivate remoteActivate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remoteActivate = GetComponent<RemoteActivate>();
        if (!setValue)
        {
            remoteActivate.SetActive(true);
            active = false;
        }
    }

    void Update()
    {
        List<Collider> toRemove = new();
        foreach (var keyValue in withinTrigger)
        {
            if (keyValue.Key == null) toRemove.Add(keyValue.Key);
        }
        if (toRemove.Count > 0)
        {
            foreach (var c in toRemove) withinTrigger.Remove(c);
            bool shouldBeActive = !active && withinTrigger.Count > 0;
            remoteActivate.SetActive(shouldBeActive ? setValue : setValue);
            active = shouldBeActive;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.isTrigger) return;
        if (!withinTrigger.ContainsKey(collider)) withinTrigger.Add(collider, true);
        if (!active && withinTrigger.Count > 0)
        {
            remoteActivate.SetActive(setValue);
            active = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (withinTrigger.ContainsKey(other)) withinTrigger.Remove(other);
        if (active && withinTrigger.Count == 0)
        {
            remoteActivate.SetActive(!setValue);
            active = false;
        }
    }
}
