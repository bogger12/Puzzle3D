using UnityEngine;

public class KillLayer : MonoBehaviour
{

    public Transform respawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered cloud");
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.GetComponent<Rigidbody>().position = respawnPoint.position;
            playerMovement.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            playerMovement.transform.position = respawnPoint.position;
        }
    }
}
