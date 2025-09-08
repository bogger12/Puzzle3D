using UnityEngine;

public class HoldableKey : Holdable
{

    private Animator animator;

    public bool inKeyHole = false;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        base.HeldBy(holdingBody, holdAnchor);
        TakeKeyFromHole();
    }

    public override void OnThrow()
    {
        base.OnThrow();
    }

    public void PutKeyInHole(Transform keyAnchor)
    {
        // Set transforms
        transform.parent = keyAnchor;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        animator.SetBool("open", true);
        rb.isKinematic = true;
        inKeyHole = true;
    }

    public void TakeKeyFromHole()
    {
        // Set transforms
        transform.parent = null;
        animator.SetBool("open", false);
        rb.isKinematic = false;
        inKeyHole = true;
    }
}
