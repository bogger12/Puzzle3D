using UnityEngine;

public class TriggerNewSpawn : MonoBehaviour
{
    public Transform respawnPoint;

    void Awake()
    {
        if (respawnPoint == null)
        {
            Debug.LogError("No Spawn point set for this zone!!");
        }
        respawnPoint.GetComponent<MeshRenderer>().enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered New Spawn Zone");
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Debug.Log("Setting New Spawn Zone");
            playerMovement.GetComponent<Respawner>().currentSpawnPoint = respawnPoint;
        }
    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, respawnPoint.position, Color.cyan);
        Gizmos.DrawWireCube(respawnPoint.position, Vector3.one);
    }
}
