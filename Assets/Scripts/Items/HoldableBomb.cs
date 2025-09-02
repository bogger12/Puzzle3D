using UnityEngine;

[RequireComponent(typeof(BombExplode))]
public class HoldableBomb : Holdable
{
    private BombExplode bombExplode;

    public void Start()
    {
        bombExplode = GetComponent<BombExplode>();
    }
    public override void SetIsHeld(bool held)
    {
        isHeld = held;
        if (held) bombExplode.StartFuse();
    }
}
