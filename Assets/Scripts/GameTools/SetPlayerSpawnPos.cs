using UnityEngine;
using UnityEngine.InputSystem;

public class SetPlayerSpawnPos : MonoBehaviour
{

    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Move the new player to the spawn position
        Respawner playerRespawner = playerInput.GetComponent<Respawner>();
        playerRespawner.currentSpawnPoint = spawnPoint;
        playerRespawner.Respawn();
        Debug.Log("Spawn player at " + spawnPoint.gameObject.name);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnPoint.position, Vector3.one);
        Debug.DrawLine(transform.position, spawnPoint.position, Color.cyan);
    }
}
