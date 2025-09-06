using UnityEngine;

[RequireComponent(typeof(BombExplode))]
public class BombBurn : Burnable
{

    BombExplode bombExplode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        bombExplode = GetComponent<BombExplode>();
    }

    public override void StartBurn()
    {
        base.StartBurn();
        // Vector2 fuseTimeRange = bombExplode.fuseTimeRangeOfAffectedBomb;
        bombExplode.StartFuse();
    }

    protected override void SetBurnAmount(float amount) { }
}
