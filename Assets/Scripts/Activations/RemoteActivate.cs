using UnityEngine;

public class RemoteActivate : MonoBehaviour
{

    public Activateable activateable;

    public bool SetActive(bool active)
    {
        if (activateable == null)
        {
            Debug.LogError("You need to set an activateable object for this RemoteActivate");
            return false;
        }
#if UNITY_EDITOR
        lineColor = active ? Color.green : Color.red;
#endif
        return activateable.SetActive(active);
    }
    public bool GetActive()
    {
        return activateable.GetActive();
    }

#if UNITY_EDITOR
    private Color lineColor = Color.red;
    void OnDrawGizmos()
    {
        if (activateable != null)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position, activateable.GetComponentInChildren<Collider>().bounds.center);
        }
    }
#endif
}
