using UnityEngine;


public class ActivateDoor : Activateable
{
    public override void SetActive(bool active) {
        Debug.Log(transform.name + " set to" + active);
    }
}