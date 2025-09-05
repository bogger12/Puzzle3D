using UnityEngine;

public abstract class Burnable : MonoBehaviour
{

    protected float burnAmount;
    public float BurnAmount
    {
        get { return burnAmount; }
        private set
        {
            burnAmount = value; SetBurnAmount(value);
        }
    }
    [SerializeField]
    protected bool burning = false;
    public float burnPerSecond = 0.5f;

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // protected void Start()
    // {
    // }

    // Update is called once per frame
    protected void Update()
    {
        if (burning)
        {
            BurnAmount += Time.deltaTime * burnPerSecond;
        }
    }

    public virtual void StartBurn()
    {
        burning = true;
    }
    public void ResetBurn()
    {
        burning = false;
        BurnAmount = 0f;
    }

    protected virtual void SetBurnAmount(float value) { }
}
