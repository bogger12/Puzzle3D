using UnityEngine;

public abstract class Activateable : MonoBehaviour
{

    public bool active = false;

    public virtual bool SetActive(bool active)
    {
        this.active = active;
        return active;
    }
}
