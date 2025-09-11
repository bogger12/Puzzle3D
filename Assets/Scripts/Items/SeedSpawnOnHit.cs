using UnityEngine;

public class SeedSpawnOnHit : MonoBehaviour
{

    public GameObject spawnObject;
    public bool rotateToNormal = true;
    public Quaternion spawnRotationOverride = Quaternion.identity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(Vector3 spawnPosition, Vector3 normal)
    {
        Quaternion rotation = rotateToNormal ? Quaternion.FromToRotation(Vector3.up, normal) : spawnRotationOverride;
        Debug.Log("should be at: " + spawnPosition);
        GameObject spawnedPlatform = Instantiate(spawnObject, spawnPosition, rotation);
        Debug.Log("New Position: " + spawnedPlatform.transform.position);
        // Debug.Break();
        // spawnObject.GetComponent<Animator>().SetBool("grow", true);
    }
}
