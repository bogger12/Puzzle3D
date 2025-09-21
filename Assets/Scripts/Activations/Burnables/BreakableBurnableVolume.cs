using System.Collections.Generic;
using UnityEngine;

public class BreakableBurnableVolume : BurnableVolume
{

    public GameObject breakParticles;

    public override void FinishBurn()
    {
        base.FinishBurn();
        GameObject breakParticlesInstance = Instantiate(breakParticles, transform.position, Quaternion.identity);
        breakParticlesInstance.transform.localScale = burnParticles.transform.localScale;
        Destroy(transform.parent.gameObject);
    }


    protected override void SetBurnAmount(float amount) { }
}
