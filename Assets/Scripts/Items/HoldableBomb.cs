using UnityEngine;

[RequireComponent(typeof(BombExplode))]
public class HoldableBomb : Holdable
{
    private BombExplode bombExplode;

    protected override void Start()
    {
        base.Start();
        bombExplode = GetComponent<BombExplode>();
    }
    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        base.HeldBy(holdingBody, holdAnchor);
        bombExplode.StartFuse();
    }
}
