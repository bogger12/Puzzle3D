using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : RemoteActivate
{
    public bool setOnActivate = true;

    private bool active = false;

    Dictionary<Collider, bool> withinTrigger = new Dictionary<Collider, bool>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!withinTrigger.ContainsKey(collider)) withinTrigger.Add(collider, true);
        if (!active && withinTrigger.Count > 0) active = SetActive(true);
        
        string items = "";
        foreach (var item in withinTrigger)
        {
            items += item.Key.name;
        }
        Debug.Log("enter: " + items);
    }

    public void OnTriggerExit(Collider other)
    {
        if (withinTrigger.ContainsKey(other)) withinTrigger.Remove(other);
        if (active && withinTrigger.Count == 0) active = SetActive(false);

        string items = "";
        foreach (var item in withinTrigger)
        {
            items += item.Key.name;
        }
        Debug.Log("exit: " + items);
    }
}
