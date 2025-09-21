using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ControlAnchorOnPoint : AnchorOn3DPoint
{
    // public Holdable currentHoldable { get; protected set; } = null;

    [Header("Oscillate Animation")]
    [Range(0,10)]
    public float oscillationSpeed = 1;
    public Vector2 sizeRange = new Vector2(0.5f, 1f);

    public TMP_Text buttonText;
    public TMP_Text hintText;

    public GameObject longPressIcon;

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(anchor != null);
    }
    protected void Update()
    {
        if (oscillationSpeed > 0)
        {
            float currentSize = Mathf.Lerp(sizeRange.x, sizeRange.y, Mathf.Sin(Time.time * oscillationSpeed) / 2f + 0.5f);
            transform.localScale = defaultScale * currentSize;
        }
    }
    public void SetHoldableTarget(Holdable holdable)
    {
        // currentHoldable = holdable;
        anchor = holdable != null ? holdable.transform : null;
        gameObject.SetActive(holdable != null);
    }

    public void SetButtonText(string text)
    {
        buttonText.SetText(text);
    }
    public void SetHintText(string text)
    {
        hintText.SetText(text);
    }
    public void SetLongPressShown(bool shown)
    {
        longPressIcon.SetActive(shown);
    }

    // public void SetTargetAndTexts(Holdable holdable, string button, string hint)
    // {
    //     SetHoldableTarget(holdable);
    //     if (holdable != null)
    //     {
    //         SetButtonText(button);
    //         SetHintText(hint);
    //         SetLongPressShown(holdable.GetIsLongPress());
    //     }
    // }
    public void SetTargetAndTextsAuto(Holdable holdable, PlayerInputStore playerInputStore)
    {
        SetHoldableTarget(holdable);
        if (holdable != null)
        {
            SetButtonText(playerInputStore.GetButtonText(playerInputStore.playerInput.actions["Pickup_Throw"]));
            SetHintText(holdable.GetControlHint());
            SetLongPressShown(holdable.GetIsLongPress());
        }
    }
}