using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateDoor : Activateable
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override bool SetActive(bool active)
    {
        base.SetActive(active);
        Debug.Log(transform.name + " set to" + active);
        animator.SetBool("open", active);
        return active;
    }
}