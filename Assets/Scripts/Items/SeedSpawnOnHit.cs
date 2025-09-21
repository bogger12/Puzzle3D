using Unity.Burst.Intrinsics;
using UnityEngine;

public class SeedSpawnOnHit : MonoBehaviour
{

    public GameObject spawnObject;
    public GameObject spawnParticles;
    public bool rotateToNormal = true;
    public bool destroySeedOnPlant = false;
    public Quaternion spawnRotationOverride = Quaternion.identity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(Transform spawnParent, Vector3 spawnPosition, Vector3 normal)
    {
        Quaternion rotation = rotateToNormal ? Quaternion.FromToRotation(Vector3.up, normal) : spawnRotationOverride;
        Debug.Log("should be at: " + spawnPosition);
        GameObject spawnedPlatform = Instantiate(spawnObject, spawnPosition, rotation);
        GameObject spawnedParticles = Instantiate(spawnParticles, spawnPosition, rotation);
        Debug.Log("New Position: " + spawnedPlatform.transform.position);
        spawnedPlatform.transform.parent = spawnParent.transform;
        if (destroySeedOnPlant) Destroy(gameObject);
        // Debug.Break();
        // spawnObject.GetComponent<Animator>().SetBool("grow", true);
    }
}
