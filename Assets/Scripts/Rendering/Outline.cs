using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    public Material outlineMaterial;
    private Material localMaterialInstance;

    [SerializeField]
    private new bool enabled;
    public bool Enabled { get { return enabled; } set { SetOutline(value); enabled = value; } }
    public bool applyToChildren = true;

    private MeshRenderer[] meshRenderers;

    private Dictionary<MeshRenderer, Material[]> initialMaterials = new Dictionary<MeshRenderer, Material[]>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        localMaterialInstance = new Material(outlineMaterial);
        if (applyToChildren) meshRenderers = GetComponentsInChildren<MeshRenderer>();
        else meshRenderers = new MeshRenderer[] { GetComponent<MeshRenderer>() };

        foreach (MeshRenderer renderer in meshRenderers)
        {
            initialMaterials[renderer] = renderer.materials;
        }
    }

    protected virtual void SetOutline(bool isenabled)
    {
        if (enabled == isenabled) return;

        foreach (MeshRenderer renderer in meshRenderers)
        {
            Material[] outlineMaterialsSet = new List<Material>(initialMaterials[renderer]) { localMaterialInstance }.ToArray();
            renderer.materials = isenabled ? outlineMaterialsSet : initialMaterials[renderer];
        }
    }

    public virtual void ChangeColor(Color color)
    {
        if (Enabled && localMaterialInstance.color != color) localMaterialInstance.color = color;
    }

}
