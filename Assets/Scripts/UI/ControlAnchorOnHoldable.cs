using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ControlAnchorOnHoldable : AnchorOn3DPoint
{
    protected Holdable currentHoldable = null;

    [Header("Oscillate Animation")]
    public float oscillationSpeed = 1;
    public Vector2 sizeRange = new Vector2(0.5f, 1f);

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(currentHoldable != null);
    }
    protected void Update()
    {
        float currentSize = Mathf.Lerp(sizeRange.x, sizeRange.y, Mathf.Sin(Time.time * oscillationSpeed) / 2f + 0.5f);
        transform.localScale = defaultScale * currentSize;
    }
    public void SetHoldableTarget(Holdable holdable)
    {
        currentHoldable = holdable;
        anchor = holdable != null ? holdable.transform : null;
        gameObject.SetActive(holdable != null);
    }
}