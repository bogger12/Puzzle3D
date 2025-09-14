using UnityEngine;


public enum KeyState
{
    None,
    UnlockTurn,
    LockTurn,
    InKeyHole
}

[RequireComponent(typeof(Animator))]
public class HoldableKey : Holdable
{

    private Animator animator;

    [HideInInspector]
    // public bool inKeyHole = false;
    private Keyhole activeKeyHole = null;


    private Rigidbody currentHoldingBody;
    private Transform currentHoldAnchor;

    private KeyState keyState = KeyState.None;

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
        bool finishedClosedAnimation = stateInfo.IsName("KeyClose");

        if (finishedClosedAnimation && keyState == KeyState.LockTurn && animator.enabled)
        {
            TakeKeyFromHole();
            if (currentHoldingBody != null && currentHoldAnchor != null)
            {
                base.HeldBy(currentHoldingBody, currentHoldAnchor);
                currentHoldingBody = null;
                currentHoldAnchor = null;
            }
            // Debug.Log("disabling key animation after finish");
        }
        else if (finishedOpenAnimation && keyState == KeyState.UnlockTurn && animator.enabled && !activeKeyHole.FinishedUnlockAnim)
        {
            activeKeyHole.FinishedKeyUnlockAnim();
        }
    }
    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        // base.HeldBy(holdingBody, holdAnchor);
        // if (activeKeyHole && inKeyHole && !activeKeyHole.allowKeyPickup) return;
        if (keyState != KeyState.None)
        {
            this.currentHoldingBody = holdingBody;
            this.currentHoldAnchor = holdAnchor;
            animator.SetBool("open", false);
            keyState = KeyState.LockTurn;
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
        if (keyState == KeyState.None && activeKeyHole)
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
        // inKeyHole = true;
        keyState = KeyState.UnlockTurn;

        activeKeyHole.Unlock();
    }

    public void TakeKeyFromHole()
    {
        // Set transforms
        transform.parent = null;
        // animator.SetBool("open", false);
        animator.enabled = false;
        rb.isKinematic = false;
        // inKeyHole = false;
        keyState = KeyState.None;
        activeKeyHole.Lock();
    }

    public void SetActiveKeyHole(Keyhole keyHole)
    {
        if (keyState == KeyState.None && activeKeyHole != keyHole)
        {
            this.activeKeyHole = keyHole;
            if (keyHole != null) AssignStaticKeyHints(true);
            else AssignStaticHints(true);
        }
        // else AssignStaticHints(false);
        // Debug.Log("Active Keyhole = " + (keyHole!=null?keyHole.name: keyHole));
    }
    public Keyhole GetActiveKeyHole()
    {
        return activeKeyHole;
    }

    public void AssignStaticKeyHints(bool display)
    {
        ControlHintsManager controlHintsManager = playerThrow.controlHintsManager;
        controlHintsManager.ResetHints();
        if (display)
        {
            PlayerInputs playerInputs = playerThrow.playerInputs;
            controlHintsManager.AssignHint(playerInputs.GetButtonText(playerInputs.holdThrow), "Unlock", false);
            controlHintsManager.AssignHint(playerInputs.GetButtonText(playerInputs.holdThrow), "Throw", true);
        }
    }
}
