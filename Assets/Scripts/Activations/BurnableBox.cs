using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BurnableBox : BurnableBurnsAdjacent
{

    public ParticleSystem burnParticles;
    public GameObject breakParticles;

    public override void Start()
    {
        base.Start();
        burnParticles.Stop();
    }

    public override void StartBurn()
    {
        base.StartBurn();
        Debug.Log(transform.parent.name + " started burning");
        burnParticles.Play();
    }

    public override void FinishBurn()
    {
        base.FinishBurn();
        burnParticles.Stop();
        Instantiate(breakParticles, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }


    protected override void SetBurnAmount(float amount) { }
}
