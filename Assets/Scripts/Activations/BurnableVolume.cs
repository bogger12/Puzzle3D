using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BurnableVolume : BurnableBurnsAdjacent
{

    public ParticleSystem burnParticles;
    public GameObject breakParticles;

    [Header("Particles")]
    public AnimationCurve emissionRateOverTime;
    public float emissionMultiplier = 10;
    public AnimationCurve speedOverTime;

    private float timer = 0;
    public ParticleSystem.EmissionModule burnEmission;
    public ParticleSystem.MainModule burnMainModule;
    private ParticleSystem.MinMaxCurve initialSpeedRange;


    public override void Start()
    {
        base.Start();
        burnParticles.Stop();
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
            timer += Time.deltaTime;

            float emission_t = emissionRateOverTime.Evaluate(timer / burnTime);
            float speed_t = speedOverTime.Evaluate(timer / burnTime);

            burnEmission.rateOverTime = emission_t * emissionMultiplier;

            ParticleSystem.MinMaxCurve currentSpeedRange = initialSpeedRange;
            currentSpeedRange.constantMin = initialSpeedRange.constantMin * speed_t;
            currentSpeedRange.constantMax = initialSpeedRange.constantMax * speed_t;
            burnMainModule.startSpeed = currentSpeedRange;

            // Debug.Log("emission: " + emission_t * emissionMultiplier);
            Debug.Log("speed: " + speed_t * new Vector2(initialSpeedRange.constantMin, initialSpeedRange.constantMax));
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
        GameObject breakParticlesInstance = Instantiate(breakParticles, transform.position, Quaternion.identity);
        breakParticlesInstance.transform.localScale = burnParticles.transform.localScale;
        Destroy(transform.parent.gameObject);
    }


    protected override void SetBurnAmount(float amount) { }
}
