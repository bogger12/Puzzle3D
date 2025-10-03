using System.Collections.Generic;
using UnityEngine;



public static class Utils
{
    public static T GetClosestObject<T>(Transform reference, IEnumerable<T> objects) where T : Component
    {
        T closest = default(T);
        float closestDistance = float.PositiveInfinity;
        foreach (T item in objects)
        {
            if (Vector3.Distance(item.transform.position, reference.position) < closestDistance)
            {
                closest = item;
            }
        }
        return closest;
    }
}