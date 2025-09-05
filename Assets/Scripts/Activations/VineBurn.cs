using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class VineBurn : Burnable
{

    MeshRenderer meshRenderer;
    Collider vineCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        vineCollider = GetComponent<Collider>();
    }

    protected override void SetBurnAmount(float value)
    {
        meshRenderer.material.SetFloat("_BurnAmount", value);
        vineCollider.enabled = value < 1f;
    }
}
