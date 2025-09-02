using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateDoor : Activateable
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void SetActive(bool active)
    {
        Debug.Log(transform.name + " set to" + active);
        animator.SetBool("open", active);
    }
}