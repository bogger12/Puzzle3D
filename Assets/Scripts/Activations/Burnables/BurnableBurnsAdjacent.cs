using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BurnableBurnsAdjacent : Burnable
{


    protected Collider nearbyBurnCollider;
    public float delayBetweenNearbyBurns = 0;
    public bool useDelayWhenAlreadyBurning = false;


    private List<Burnable> burnablesInsideBurnVolume = new List<Burnable>();
    public virtual void Start()
    {
        nearbyBurnCollider = GetComponent<Collider>();
    }
    public override void StartBurn()
    {
        burning = true;
        // Start burning nearby objects
        
        if (burnablesInsideBurnVolume.Count>0) foreach (Burnable b in burnablesInsideBurnVolume)
        {
            if (!b.burning)
            {
                StartCoroutine(StartBurnAfter(delayBetweenNearbyBurns, b));
                // Debug.Log(transform.parent.name + "burns " + b.transform.parent.name + " after start burning");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        Burnable burnable = other.GetComponentInChildren<Burnable>();
        if (burning && burnable!=null)
        {
            float delay = useDelayWhenAlreadyBurning ? delayBetweenNearbyBurns : 0;
            if (!burnable.burning)
            {
                StartCoroutine(StartBurnAfter(delay, burnable));
                // Debug.Log(transform.parent.name + "burns " + burnable.transform.parent.name);
            }
        } else if (burnable!=null) {
            burnablesInsideBurnVolume.Add(burnable);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        Burnable burnable = other.GetComponentInChildren<Burnable>();
        if (burnable!=null)
        {
            burnablesInsideBurnVolume.Remove(burnable);
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
