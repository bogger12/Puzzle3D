using UnityEngine;
using UnityEngine.InputSystem;

public class SetPlayerSpawnPos : MonoBehaviour
{

    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Move the new player to the spawn position
        playerInput.transform.position = spawnPoint.position;
        playerInput.GetComponent<Rigidbody>().position = spawnPoint.position;
        Debug.Log("Move player");
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnPoint.position, Vector3.one);
    }
}
