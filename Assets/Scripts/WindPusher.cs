using System.Collections.Generic;
using UnityEngine;

public class WindPusher : MonoBehaviour
{

    public float pushForce;

    public Vector3 customDirection = Vector3.zero;

    public AnimationCurve speedOverTime;

    private Dictionary<Rigidbody, float> speedOfContainedBodies = new();

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
        if (other.attachedRigidbody)
        {
            speedOfContainedBodies.TryAdd(other.attachedRigidbody, 0f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            speedOfContainedBodies[other.attachedRigidbody] = speedOfContainedBodies[other.attachedRigidbody] + Time.deltaTime;
            Vector3 direction = Vector3.Normalize(customDirection != Vector3.zero ? customDirection : transform.up);

            float speed = speedOverTime.Evaluate(speedOfContainedBodies[other.attachedRigidbody]) * pushForce;
            other.attachedRigidbody.MovePosition(other.attachedRigidbody.position + speed * Time.deltaTime * direction);
            // other.attachedRigidbody.AddForce(pushForce * direction, ForceMode.VelocityChange);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            speedOfContainedBodies.Remove(other.attachedRigidbody);
        }
    }
}
