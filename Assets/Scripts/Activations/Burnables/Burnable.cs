using UnityEngine;

public abstract class Burnable : MonoBehaviour
{

    private float burnAmount;
    public float BurnAmount
    {
        get { return burnAmount; }
        protected set
        {
            burnAmount = value; SetBurnAmount(value);
        }
    }
    [HideInInspector]
    public bool burning = false;
    public float burnTime = 1f;

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // protected void Start()
    // {
    // }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (burning)
        {
            BurnAmount = Mathf.Clamp01(BurnAmount + Time.deltaTime / burnTime);
            if (burnAmount >= 1) FinishBurn();
        }
    }

    public virtual void StartBurn()
    {
        burning = true;
    }

    public virtual void FinishBurn()
    {
        burning = false;
    }
    public virtual void ResetBurn()
    {
        burning = false;
        BurnAmount = 0f;
    }

    protected abstract void SetBurnAmount(float value);
}
