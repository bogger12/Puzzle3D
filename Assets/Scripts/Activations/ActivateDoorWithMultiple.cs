using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateDoorWithMultiple : ActivateAnimateable
{
    public bool permanentlyOpens = false;
    public int activationsNeeded = 2;
    public bool needsExactNumActivations = false;
    private int numActivated = 0;

    public override void SetActive(bool active)
    {
        numActivated = active ? numActivated + 1 : numActivated - 1;
        Debug.Log(transform.name + " set to" + active + "numActive: " + numActivated);

        bool doorActive = (needsExactNumActivations ? numActivated == activationsNeeded : numActivated >= activationsNeeded) || (permanentlyOpens && this.active);
        base.SetActive(doorActive);
    }
}