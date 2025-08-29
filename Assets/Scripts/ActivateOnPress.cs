using UnityEngine;

public class ActivateOnPress : MonoBehaviour
{

    public Activateable activateable;

    public void SetActive(bool active)
    {
        activateable.SetActive(active);
    }
}
