using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    public Material outlineMaterial;

    [SerializeField]
    private new bool enabled;
    public bool Enabled { get { return enabled; } set { enabled = SetOutline(value); } }
    public bool applyToChildren = true;

    public MeshRenderer[] meshRenderers;

    private Dictionary<MeshRenderer, Material[]> initialMaterials = new Dictionary<MeshRenderer, Material[]>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (applyToChildren) meshRenderers = GetComponentsInChildren<MeshRenderer>();
        else meshRenderers = new MeshRenderer[] { GetComponent<MeshRenderer>() };

        foreach (MeshRenderer renderer in meshRenderers) {
            initialMaterials[renderer] = renderer.materials;
        }
    }

    bool SetOutline(bool isenabled)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            Material[] outlineMaterialsSet = new List<Material>(initialMaterials[renderer]) { outlineMaterial }.ToArray();
            renderer.materials = isenabled ? outlineMaterialsSet :  initialMaterials[renderer];
        }
        return isenabled;
    }

}
