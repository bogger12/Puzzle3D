using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RemoteActivate))]
public class PressurePlate : MonoBehaviour
{
    public bool setValue = true;

    private bool active = false;

    Dictionary<Collider, bool> withinTrigger = new Dictionary<Collider, bool>();

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

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.isTrigger) return;
        if (!withinTrigger.ContainsKey(collider)) withinTrigger.Add(collider, true);
        if (!active && withinTrigger.Count > 0)
        {
            remoteActivate.SetActive(setValue);
            active = true;
        }
        
        // string items = "";
            // foreach (var item in withinTrigger)
            // {
            //     items += item.Key.name;
            // }
            // Debug.Log("enter: " + items);
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

        // string items = "";
            // foreach (var item in withinTrigger)
            // {
            //     items += item.Key.name;
            // }
            // Debug.Log("exit: " + items);
        }
}
