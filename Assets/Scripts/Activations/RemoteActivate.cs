using UnityEngine;

public class RemoteActivate : MonoBehaviour
{

    public Activateable activateable;

    public bool SetActive(bool active)
    {
        return activateable.SetActive(active);
    }
}
