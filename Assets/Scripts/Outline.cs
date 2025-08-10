using UnityEngine;

public class Outline : MonoBehaviour
{

    public Material outlineMaterial;

    [SerializeField]
    private new bool enabled;
    public bool Enabled { get { return enabled; } set { enabled = SetOutline(value); } }

    private MeshRenderer meshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    bool SetOutline(bool isenabled)
    {
        Material[] newMaterials = { meshRenderer.materials[0], isenabled ? outlineMaterial : null };
        meshRenderer.materials = newMaterials;
        return isenabled;
    }

}
