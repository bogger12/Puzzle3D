using UnityEngine;

public class RemoteActivate : MonoBehaviour
{

    public Activateable activateable;

    public bool SetActive(bool active)
    {
#if UNITY_EDITOR
        lineColor = active ? Color.green : Color.red;
#endif
        return activateable.SetActive(active);
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
