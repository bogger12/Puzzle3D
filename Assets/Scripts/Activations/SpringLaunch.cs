using UnityEngine;

public class SpringLaunch : MonoBehaviour
{

    public float launchForce = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody body))
        {
            body.linearVelocity = transform.localRotation * new Vector3(body.linearVelocity.x, 0, body.linearVelocity.z);
            body.AddForce(transform.localRotation * Vector3.up * launchForce, ForceMode.VelocityChange);
        }
    }
}
