using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Respawner : MonoBehaviour
{

    public Transform currentSpawnPoint;

    public void Respawn()
    {
        transform.position = currentSpawnPoint.position;
        GetComponent<Rigidbody>().position = currentSpawnPoint.position;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("KillPlayer"))
        {
            Respawn();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("KillPlayer"))
        {
            Respawn();
        }
    }
}
