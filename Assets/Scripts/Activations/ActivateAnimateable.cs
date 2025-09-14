using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateAnimateable : Activateable
{
    protected Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
        animator.SetBool("active", active);
    }
}