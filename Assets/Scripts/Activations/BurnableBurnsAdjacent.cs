using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BurnableBurnsAdjacent : Burnable
{


    protected BoxCollider nearbyBurnCollider;
    public float delayBetweenNearbyBurns = 0;
    public virtual void Start()
    {
        nearbyBurnCollider = GetComponent<BoxCollider>();
    }
    public override void StartBurn()
    {
        burning = true;
        // Start burning nearby objects
        Collider[] nearbyColliders = Physics.OverlapBox(nearbyBurnCollider.transform.position, nearbyBurnCollider.bounds.extents);

        foreach (Collider c in nearbyColliders)
        {
            if (c.TryGetComponent<Burnable>(out Burnable burnable))
                if (!burnable.burning) StartCoroutine(StartBurnAfter(delayBetweenNearbyBurns, burnable));
        }
    }

    private IEnumerator<WaitForSeconds> StartBurnAfter(float duration, Burnable b)
    {
        // Eventual code to execute right as the function is called
        yield return new WaitForSeconds(duration);
        // The code from here will be executed after **duration** seconds
        if (b == null) yield break;
        b.StartBurn();
    }
}
