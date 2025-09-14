using UnityEngine;

public class RemoteActivate : MonoBehaviour
{

    public Activateable activateable;

    public virtual void SetActive(bool active)
    {
        if (activateable == null)
        {
            Debug.LogError("You need to set an activateable object for this RemoteActivate");
        }
#if UNITY_EDITOR
        lineColor = active ? Color.green : Color.red;
#endif
        activateable.SetActive(active);
    }
    public virtual bool GetActive()
    {
        return activateable.GetActive();
    }

#if UNITY_EDITOR
    protected Color lineColor = Color.red;
    protected virtual void OnDrawGizmos()
    {
        if (activateable != null)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position, activateable.GetComponentInChildren<Collider>().bounds.center);
        }
    }
#endif
}
