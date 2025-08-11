using UnityEngine;

public class VineBurn : MonoBehaviour
{

    private float burnAmount;
    public float BurnAmount
    {
        get { return burnAmount; }
        private set
        {
            burnAmount = value; SetBurnAmount(value);
        }
    }
    [SerializeField]
    private bool burning = false;
    public float burnPerSecond = 0.5f;

    MeshRenderer meshRenderer;
    Collider vineCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        vineCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (burning)
        {
            BurnAmount += Time.deltaTime * burnPerSecond;
        }
    }

    public void StartBurn()
    {
        burning = true;
    }
    public void ResetBurn()
    {
        burning = false;
        BurnAmount = 0f;
    }

    private void SetBurnAmount(float value)
    {
        meshRenderer.material.SetFloat("_BurnAmount", burnAmount);
        vineCollider.enabled = burnAmount < 1f;
    }
}
