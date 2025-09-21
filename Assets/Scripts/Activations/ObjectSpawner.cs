using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : Activateable
{

    public Transform spawnAnchor;
    public List<GameObject> objectsToSpawn;
    public bool spawnOnStart = true;
    public bool oneInstanceAllowed = true;
    public Vector3 velocityAppliedOnSpawn = Vector3.zero;

    private List<GameObject> currentObjects = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnOnStart) SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
        if (active) // Spawn Item
        {
            if (oneInstanceAllowed && currentObjects.Count > 0) DestroyObjects();
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        foreach (GameObject ob in objectsToSpawn)
        {
            GameObject spawnedObject = Instantiate(ob, spawnAnchor.position, transform.rotation);
            if (velocityAppliedOnSpawn.magnitude > 0 && spawnedObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.AddForce(velocityAppliedOnSpawn, ForceMode.VelocityChange);
            currentObjects.Add(spawnedObject);
        }
    }

    private void DestroyObjects()
    {
        foreach (GameObject ob in currentObjects) Destroy(ob);
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(spawnAnchor.position, velocityAppliedOnSpawn, Color.cyan);
    }
}
