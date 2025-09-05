using UnityEngine;

public class RemoteActivate : MonoBehaviour
{

    public Activateable activateable;

    public bool SetActive(bool active)
    {
        return activateable.SetActive(active);
    }

    void OnDrawGizmos()
    {
        if (activateable != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, activateable.GetComponentInChildren<Collider>().bounds.center);
        }
    }
}
