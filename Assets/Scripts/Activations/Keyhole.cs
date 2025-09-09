using UnityEngine;

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

    public bool Locked { get; private set; } = true;
    public bool FinishedUnlockAnim { get; private set; } = false;

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

        if (!setActiveOnlyWhenFinishedAnim)
        {
            Locked = false;
            SetActive(setValue);
        }
        else if (disableOutlineOnUnlock) outline.Enabled = false;
    }

    public void FinishedKeyUnlockAnim()
    {
        Debug.Log("Finished Unlock Animation!");
        FinishedUnlockAnim = true;
        if (setActiveOnlyWhenFinishedAnim)
        {
            Locked = false;
            SetActive(setValue);
        }
        outline.Enabled = false;
    }

    public void Lock()
    {
        Debug.Log("Locked.");
        Locked = true;
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
            if (!Locked && !disableOutlineOnUnlock)
            {
                outline.Enabled = true;
                outline.ChangeColor(unlockedColor);
            }
            else
            {
                HoldableKey playerHeldKey = playerThrow.heldBody != null ? playerThrow.heldBody.GetComponent<HoldableKey>() : null;
                bool hasKey = playerHeldKey != null;
                if (hasKey && currentKey != playerHeldKey)
                {
                    currentKey = playerHeldKey;
                    playerHeldKey.SetActiveKeyHole(this);
                }
                    outline.Enabled = (!disableOutlineOnUnlock || Locked);
                outline.ChangeColor(hasKey ? hasKeyColor : noKeyColor);
            }
        }
    }

    void OnTriggerStay(Collider other) // It seems like this tanks FPS
    {
        if (!Locked && !disableOutlineOnUnlock)
        {
            outline.Enabled = false;
            return;
        }
        if (other.TryGetComponent<PlayerThrow>(out PlayerThrow playerThrow)) // if it is the player
            {
                if (!Locked && !disableOutlineOnUnlock)
                {
                    outline.Enabled = true;
                    outline.ChangeColor(unlockedColor);
                }
                else
                {
                    HoldableKey playerHeldKey = playerThrow.heldBody != null ? playerThrow.heldBody.GetComponent<HoldableKey>() : null;
                    bool hasKey = playerHeldKey != null;
                    if (hasKey && currentKey != playerHeldKey)
                    {
                        currentKey = playerHeldKey;
                        playerHeldKey.SetActiveKeyHole(this);
                    }
                    outline.Enabled = (!disableOutlineOnUnlock || Locked);
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
            HoldableKey playerHeldKey = playerThrow.heldBody != null ? playerThrow.heldBody.GetComponent<HoldableKey>() : null;
            bool hasKey = playerHeldKey != null;
            if (hasKey)
            {
                currentKey = null;
                playerHeldKey.SetActiveKeyHole(null);
            }
            outline.Enabled = false;
        }
    }
}
