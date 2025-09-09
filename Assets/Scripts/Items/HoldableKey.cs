using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HoldableKey : Holdable
{

    private Animator animator;

    [HideInInspector]
    public bool inKeyHole = false;
    private Keyhole activeKeyHole = null;


    private Rigidbody currentHoldingBody;
    private Transform currentHoldAnchor;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    protected void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool finishedOpenAnimation = stateInfo.IsName("KeyOpen") && stateInfo.normalizedTime >= 1f && !stateInfo.loop;
        if (!animator.IsInTransition(0) && !inKeyHole && animator.enabled)
        {
            TakeKeyFromHole();
            if (currentHoldingBody != null && currentHoldAnchor != null)
            {
                base.HeldBy(currentHoldingBody, currentHoldAnchor);
                currentHoldingBody = null;
                currentHoldAnchor = null;
            }
            Debug.Log("disabling key animation after finish");
        }
        else if (finishedOpenAnimation && inKeyHole && animator.enabled && !activeKeyHole.FinishedUnlockAnim)
        {
            activeKeyHole.FinishedKeyUnlockAnim();
        }
    }
    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        // base.HeldBy(holdingBody, holdAnchor);
        // if (activeKeyHole && inKeyHole && !activeKeyHole.allowKeyPickup) return;
        if (inKeyHole)
        {
            this.currentHoldingBody = holdingBody;
            this.currentHoldAnchor = holdAnchor;
            animator.SetBool("open", false);
            inKeyHole = false;
        }
        else base.HeldBy(holdingBody, holdAnchor);
        // if (inKeyHole) TakeKeyFromHole();
    }

    public override void OnThrow(float physicsIgnoreTime)
    {
        base.OnThrow(physicsIgnoreTime);
    }

    public override void OnInteractDrop(Transform holdPoint, Transform dropPoint, float dropTime)
    {
        if (!inKeyHole && activeKeyHole)
        {
            base.OnThrow(0f);
            PutKeyInHole(activeKeyHole.keyAnchor);
        }
        else base.OnInteractDrop(holdPoint, dropPoint, dropTime);
    }

    public void PutKeyInHole(Transform keyAnchor)
    {
        // Set transforms
        transform.parent = keyAnchor;
        transform.localPosition = Vector3.zero;
        animator.enabled = true;
        animator.SetBool("open", true);
        rb.isKinematic = true;
        inKeyHole = true;

        activeKeyHole.Unlock();
    }

    public void TakeKeyFromHole()
    {
        // Set transforms
        transform.parent = null;
        // animator.SetBool("open", false);
        animator.enabled = false;
        rb.isKinematic = false;
        inKeyHole = false;
        activeKeyHole.Lock();
    }

    public void SetActiveKeyHole(Keyhole keyHole)
    {
        if (!inKeyHole && activeKeyHole!=keyHole) this.activeKeyHole = keyHole;
        Debug.Log("Active Keyhole = " + (keyHole!=null?keyHole.name: keyHole));
    }
    public Keyhole GetActiveKeyHole()
    {
        return activeKeyHole;
    }
}
