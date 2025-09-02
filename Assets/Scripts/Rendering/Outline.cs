using UnityEngine;

public class Outline : MonoBehaviour
{

    public Material outlineMaterial;

    [SerializeField]
    private new bool enabled;
    public bool Enabled { get { return enabled; } set { enabled = SetOutline(value); } }
    public bool applyToChildren = true;

    public MeshRenderer[] meshRenderers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (applyToChildren) meshRenderers = GetComponentsInChildren<MeshRenderer>();
        else meshRenderers = new MeshRenderer[] { GetComponent<MeshRenderer>() };
    }

    bool SetOutline(bool isenabled)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            Material[] outlineMaterialsSet = { renderer.materials[0], outlineMaterial };
            Material[] regularMaterialsSet = { renderer.materials[0] };
            renderer.materials = isenabled ? outlineMaterialsSet :  regularMaterialsSet;
        }
        return isenabled;
    }

}
