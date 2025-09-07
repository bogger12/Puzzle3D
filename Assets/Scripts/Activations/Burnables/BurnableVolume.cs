using System.Collections.Generic;
using UnityEngine;

public class BurnableVolume : BurnableBurnsAdjacent
{

    public ParticleSystem burnParticles;

    [Header("Particles")]
    public AnimationCurve emissionRateOverTime;
    public float emissionMultiplier = 10;
    public AnimationCurve speedOverTime;

    public ParticleSystem.EmissionModule burnEmission;
    public ParticleSystem.MainModule burnMainModule;
    private ParticleSystem.MinMaxCurve initialSpeedRange;


    public override void Start()
    {
        base.Start();
        // burnParticles.Stop();
        burnEmission = burnParticles.emission;
        burnMainModule = burnParticles.main;
        initialSpeedRange = burnMainModule.startSpeed;
        burnEmission.rateOverTime = 0;
    }

    protected override void Update()
    {
        base.Update();
        if (burning)
        {

            float emission_t = emissionRateOverTime.Evaluate(BurnAmount);
            float speed_t = speedOverTime.Evaluate(BurnAmount);

            burnEmission.rateOverTime = emission_t * emissionMultiplier;

            ParticleSystem.MinMaxCurve currentSpeedRange = initialSpeedRange;
            currentSpeedRange.constantMin = initialSpeedRange.constantMin * speed_t;
            currentSpeedRange.constantMax = initialSpeedRange.constantMax * speed_t;
            burnMainModule.startSpeed = currentSpeedRange;
        }
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
    }

    public override void ResetBurn()
    {
        base.ResetBurn();
        burnParticles.Stop();
    }


    protected override void SetBurnAmount(float amount) { }
}
