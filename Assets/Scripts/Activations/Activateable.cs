using UnityEngine;

public abstract class Activateable : MonoBehaviour
{

    [HideInInspector]
    public bool active = false;

    public virtual void SetActive(bool active)
    {
        this.active = active;
        if (TryGetComponent<PlaySoundOnActivate>(out PlaySoundOnActivate playSound))
        {
            playSound.PlaySound();
        }
    }
    public virtual bool GetActive()
    {
        return active;
    }
}
