using UnityEngine;

public abstract class Activateable : MonoBehaviour
{

    public bool active;

    public abstract void SetActive(bool active);
}
