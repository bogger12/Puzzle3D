using UnityEngine;


public enum LockStates
{
    Locked,
    Unlocking,
    Unlocked
}



[RequireComponent(typeof(Collider))]
public class Keyhole : RemoteActivate
{

    public Transform keyAnchor;
    private Outline outline;

    public Color noKeyColor = Color.red;
    public Color hasKeyColor = Color.green;
    public Color unlockedColor = Color.yellow;
    public bool disableOutlineOnUnlock = false;
    public bool setActiveOnlyWhenFinishedAnim = false;

    private HoldableKey currentKey = null;

    public LockStates LockState { get; private set; } = LockStates.Locked;
    public bool FinishedUnlockAnim { get; private set; } = false;
    public bool allowKeyPickup = true;

    public bool setValue = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // animator = GetComponent<Animator>();
        outline = GetComponentInParent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Unlock()
    {
        Debug.Log("Unlocked!");
        if (!allowKeyPickup) currentKey.canBeHeld = false;

        if (!setActiveOnlyWhenFinishedAnim)
        {
            Debug.Log("Set " + activateable.name + " to " + setValue);
            LockState = LockStates.Unlocked;
            SetActive(setValue);
            if (disableOutlineOnUnlock) outline.Enabled = false;
        }
        else LockState = LockStates.Unlocking;
        
    }

    public void FinishedKeyUnlockAnim()
    {
        Debug.Log("Finished Unlock Animation!");
        FinishedUnlockAnim = true;
        if (setActiveOnlyWhenFinishedAnim)
        {
            Debug.Log("Set " + activateable.name + " to " + setValue);
            LockState = LockStates.Unlocked;
            SetActive(setValue);
        }
        // outline.Enabled = false;
    }

    public void Lock()
    {
        Debug.Log("Locked.");
        LockState = LockStates.Locked;
        FinishedUnlockAnim = false;
        SetActive(!setValue);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HoldableKey>(out HoldableKey holdableKey))
        {
            currentKey = holdableKey;
            holdableKey.SetActiveKeyHole(this);
        }
        
        if (other.TryGetComponent<PlayerThrow>(out PlayerThrow playerThrow)) // if it is the player
        {
            if (LockState != LockStates.Locked && !disableOutlineOnUnlock)
            {
                outline.Enabled = true;
                outline.ChangeColor(unlockedColor);
            }
            else
            {
                HoldableKey playerHeldKey = playerThrow.HeldBodyHoldable is HoldableKey key ? key : null;
                bool hasKey = playerHeldKey != null;
                if (hasKey && currentKey != playerHeldKey)
                {
                    currentKey = playerHeldKey;
                    playerHeldKey.SetActiveKeyHole(this);
                }
                    outline.Enabled = (!disableOutlineOnUnlock || LockState == LockStates.Locked);
                outline.ChangeColor(hasKey ? hasKeyColor : noKeyColor);
            }
        }
    }

    void OnTriggerStay(Collider other) // It seems like this tanks FPS
    {
        if (LockState != LockStates.Locked && disableOutlineOnUnlock)
        {
            outline.Enabled = false;
            return;
        }
        if (other.TryGetComponent<PlayerThrow>(out PlayerThrow playerThrow)) // if it is the player
            {
                if (LockState != LockStates.Locked && !disableOutlineOnUnlock)
                {
                    outline.Enabled = true;
                    outline.ChangeColor(unlockedColor);
                }
                else
                {
                    HoldableKey playerHeldKey = playerThrow.HeldBodyHoldable is HoldableKey key ? key : null;
                    bool hasKey = playerHeldKey != null;
                    if (hasKey && currentKey != playerHeldKey)
                    {
                        currentKey = playerHeldKey;
                        playerHeldKey.SetActiveKeyHole(this);
                    }
                    outline.Enabled = (!disableOutlineOnUnlock || LockState == LockStates.Locked);
                    outline.ChangeColor(hasKey ? hasKeyColor : noKeyColor);
                }
            }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HoldableKey>(out HoldableKey holdableKey) && holdableKey.GetActiveKeyHole() == this)
        {
            currentKey = null;
            holdableKey.SetActiveKeyHole(null);
            // outline.Enabled = false;
        }
        if (other.TryGetComponent<PlayerThrow>(out PlayerThrow playerThrow)) // if it is the player
        {
            HoldableKey playerHeldKey = playerThrow.HeldBodyHoldable is HoldableKey key ? key : null;
            bool hasKey = playerHeldKey != null;
            if (hasKey && playerHeldKey.heldStatus==HoldableStatus.Held)
            {
                currentKey = null;
                playerHeldKey.SetActiveKeyHole(null);
            }
            outline.Enabled = false;
        }
    }
}
