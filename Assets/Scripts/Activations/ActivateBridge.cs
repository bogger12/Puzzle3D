using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateBridge : Activateable
{
    private Animator animator;

    public void Start()
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