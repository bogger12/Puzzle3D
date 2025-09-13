using System.Collections.Generic;
using UnityEngine;

public class WindPusher : MonoBehaviour
{

    public float pushForce;
    public float playerPushForce;

    public Vector3 customDirection = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            float maxSpeed = 10f;
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
                maxSpeed = movement.movementSpeed;
            Vector3 direction = Vector3.Normalize(customDirection != Vector3.zero ? customDirection : transform.up);

            // float speed = pushForce;
            // other.attachedRigidbody.MovePosition(other.attachedRigidbody.position + speed * Time.deltaTime * direction);
            Debug.Log("Current velocity: " + other.attachedRigidbody.linearVelocity);
            if ((Vector3.Dot(other.attachedRigidbody.linearVelocity,direction)*direction).magnitude < maxSpeed)
            {
                Debug.Log("adding force");
                if (movement != null) movement.AddVelocity(playerPushForce * Time.deltaTime * direction);
                else other.attachedRigidbody.AddForce(pushForce * Time.deltaTime * direction, ForceMode.Acceleration);
            }
        }
    }

}
