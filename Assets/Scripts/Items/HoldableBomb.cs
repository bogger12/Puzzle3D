using UnityEngine;

[RequireComponent(typeof(BombExplode))]
public class HoldableBomb : Holdable
{
    private BombExplode bombExplode;

    public void Start()
    {
        bombExplode = GetComponent<BombExplode>();
    }
    public override void HeldBy(Rigidbody holdingBody)
    {
        base.HeldBy(holdingBody);
        bombExplode.StartFuse();
    }
}
