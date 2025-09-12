using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterfallDisplacement : MonoBehaviour
{
    public LayerMask displacingTypes;

    public List<Transform> insideWaterfall;

    public const int MAX_OBJECTS = 4;


    private MeshRenderer meshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        insideWaterfall = new List<Transform>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log(insideWaterfall);
        Vector4[] displacementObjects = new Vector4[MAX_OBJECTS];
        for (int i=0;i<MAX_OBJECTS;i++) {
            if (insideWaterfall.Count > i) displacementObjects[i] = (Vector4)insideWaterfall[i].position;
            else displacementObjects[i] = Vector4.zero;
        }
        meshRenderer.material.SetVectorArray("_DisplacementObjects", displacementObjects);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " Entered waterfall");
        if (other.isTrigger || other.attachedRigidbody == null) return;
        Burnable burnable = other.GetComponentInChildren<Burnable>();
        if (burnable != null)
        {
            Debug.Log("Reset burn of " + burnable.transform.parent);
            burnable.ResetBurn();
            // TODO: Play put out sound effect
        }
        if ((displacingTypes.value & (1 << other.transform.gameObject.layer)) > 0) // Is correct layer
        {
            insideWaterfall.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger || other.attachedRigidbody == null) return;
        if ((displacingTypes.value & (1 << other.transform.gameObject.layer)) > 0) // Is correct layer
        {
            insideWaterfall.Remove(other.transform); // Remove Item from list
        }
    }
}
