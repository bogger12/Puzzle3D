using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateDoorWithMultiple : Activateable
{
    private Animator animator;

    public int activationsNeeded = 2;
    private int numActivated = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override bool SetActive(bool active)
    {
        numActivated = active ? numActivated + 1 : numActivated - 1;
        Debug.Log(transform.name + " set to" + active + "numActive: " + numActivated);

        bool doorActive = numActivated == activationsNeeded;
        animator.SetBool("open", doorActive);
        base.SetActive(doorActive);
        return active;
    }
}