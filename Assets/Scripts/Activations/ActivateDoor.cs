using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateDoor : Activateable
{
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override bool SetActive(bool active)
    {
        base.SetActive(active);
        Debug.Log(transform.name + " set to" + active);
        Debug.Log(animator.name);
        animator.SetBool("open", active);
        return active;
    }
}