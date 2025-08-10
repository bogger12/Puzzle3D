using UnityEngine;

public class WalkParticleController : MonoBehaviour
{
    public ParticleSystem walkParticleSystem;
    public float particleEmissionMultiplier = 1;

    private PlayerMovement playerMovementController;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovementController = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 planeVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = Vector3.Magnitude(planeVelocity);
        float verticalVelocity = rb.linearVelocity.y;

        bool grounded = playerMovementController.IsGrounded;
        var emissionModule = walkParticleSystem.emission;
        emissionModule.rateOverTime = grounded ? speed * particleEmissionMultiplier : 0;
    }

    // Vector3 VectorRemoveY(Vector3 vector) {
    //     return new Vector3(vector.x, 0, vector.z);
    // }
}
