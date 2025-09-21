using System.Collections.Generic;
using UnityEngine;

public class RemoteActivateMultiple : RemoteActivate
{
    [HideInInspector]
    private new readonly Activateable activateable; // Hide base class member
    public List<Activateable> activateables = new();

    public override void SetActive(bool active)
    {
        if (activateables.Count == 0)
        {
            Debug.LogError("You need to set an activateable object for this RemoteActivate");
        }
#if UNITY_EDITOR
        lineColor = active ? Color.green : Color.red;
#endif
        foreach (Activateable a in activateables)
        {
            a.SetActive(active);
        }
    }
    public override bool GetActive()
    {
        return activateables[0].GetActive();
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        foreach (Activateable a in activateables)
        {
            Gizmos.DrawLine(transform.position, a.GetComponentInChildren<Collider>().bounds.center);
        }
    }
#endif
}
