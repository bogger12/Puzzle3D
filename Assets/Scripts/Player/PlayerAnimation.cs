using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerAnimation : MonoBehaviour
{
    public Animator characterAnimator;

    private PlayerMovement playerMovement;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        characterAnimator.SetBool("Grounded", playerMovement.IsGrounded);
        characterAnimator.SetFloat("Vertical Velocity", Vector3.Project(rb.linearVelocity, playerMovement.upAxis).magnitude);

        // Horizontal is how far the transform rotation is away from the velocity
        float horizontal = Vector3.SignedAngle(rb.rotation * Vector3.forward, rb.linearVelocity, playerMovement.upAxis) / 90;
        Debug.DrawRay(transform.position, rb.rotation * Vector3.forward * 10, Color.red);
        Debug.DrawRay(transform.position, rb.linearVelocity * 10, Color.blue);

        characterAnimator.SetFloat("Horizontal", horizontal);

        characterAnimator.SetFloat("Speed", rb.linearVelocity.magnitude/playerMovement.movementSpeed);
        

    }
}
