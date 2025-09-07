using UnityEngine;

public class TorchBurnable : BurnableVolume
{

    public bool burnOnStart = true;

    private Light torchLight;


    public override void Start()
    {
        base.Start();
        torchLight = transform.parent.GetComponentInChildren<Light>();

        if (burnOnStart)
        {
            StartBurn();
            BurnAmount = 1;
        }
        else ResetBurn();
    }

    public override void StartBurn()
    {
        base.StartBurn();
        torchLight.enabled = true;
    }

    public override void FinishBurn()
    {
        // base.FinishBurn();
        // torchLight.enabled = false;
    }
    public override void ResetBurn()
    {
        base.ResetBurn();
        torchLight.enabled = false;
    }

    protected override void SetBurnAmount(float value) { }
}
