using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerAnimation : MonoBehaviour
{
    public Animator characterAnimator;
    public float velocityFollowSpeed = 1;
    public bool lookTowardsCamera = true;

    private PlayerMovement playerMovement;
    private Rigidbody rb;

    public AnimationCurve speedRemap;

    private Vector3 currentVelocityDirection = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        currentVelocityDirection = Vector3.Slerp(currentVelocityDirection, rb.linearVelocity.normalized, velocityFollowSpeed * Time.deltaTime);
        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, playerMovement.movementUpAxis);
        bool isMoving = horizontalVelocity.magnitude > 0.01;

        characterAnimator.SetBool("Grounded", playerMovement.IsGrounded);
        characterAnimator.SetFloat("Vertical Velocity", Vector3.Project(rb.linearVelocity, playerMovement.upAxis).magnitude);

        // Horizontal is how far the transform rotation is away from the camera direction
        float horizontalDirection = Mathf.Sign(Vector3.SignedAngle(rb.rotation * Vector3.forward, playerMovement.forwardAxis, playerMovement.upAxis)) * (lookTowardsCamera ? 1 : -1);
        float horizontal = Mathf.Abs(Vector3.Dot(rb.rotation * Vector3.forward, playerMovement.rightAxis)) * horizontalDirection;
        Debug.DrawRay(transform.position, -playerMovement.forwardAxis * 10, Color.magenta);
        Debug.DrawRay(transform.position, rb.linearVelocity * 2, Color.white);

        characterAnimator.SetFloat("Horizontal", horizontal);

        float speedRatio = isMoving ? horizontalVelocity.magnitude / playerMovement.movementSpeed : 0;
        characterAnimator.SetFloat("Speed", speedRemap.Evaluate(speedRatio));
        

    }
}
