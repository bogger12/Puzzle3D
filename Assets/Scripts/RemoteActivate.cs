using UnityEngine;

public class RemoteActivate : MonoBehaviour
{

    public Activateable activateable;

    public void SetActive(bool active)
    {
        activateable.SetActive(active);
    }
}
