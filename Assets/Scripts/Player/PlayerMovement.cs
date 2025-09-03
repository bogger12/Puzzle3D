using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    public Transform playerInputSpace = default;
    public float movementSpeed = 5f;
    public float groundedRotationSpeed = 10f;
    public float airRotationSpeed = 10f;

    public float gravityAcceleration = 9.81f;

    public float jumpForce = 2f;
    public float jumpExtraAcceleration = 2f;

    public LayerMask groundDetectionLayer;
    public float groundDetectionDistance = 1;

    [SerializeField, Range(0f, 50)]
    public float maxAcceleration = 10f, maxAirAcceleration = 1f;
    [SerializeField, Range(0f, 50)]
    public float maxDeceleration = 10f, maxAirDeceleration = 1f;

    // [Header("Debug References")]
    // public Image groundedIndicator;
    // public TextMeshProUGUI debugText;

    private Rigidbody rb;
    public bool IsGrounded { get; private set; }
    private Vector3 moveDirection;

    Vector3 upAxis, rightAxis, forwardAxis;
    Vector3 desiredVelocity;
    bool desiredJump;

    [System.Serializable]
    public class PlayerValues
    {
        public float numJumps = 1;
    }
    public PlayerValues playerValues = new PlayerValues();

    private class TrackingValues
    {
        public float timeSinceJump = 0f;
        public float numJumps = 1;
        public TrackingValues(PlayerValues playerValues)
        {
            this.numJumps = playerValues.numJumps;
        }
    }
    private TrackingValues p;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        p = new TrackingValues(playerValues);
        upAxis = Vector3.up;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        float verticalSpeed = Vector3.Dot(rb.linearVelocity, upAxis);
        Vector3 gravity = Vector3.down * gravityAcceleration;

        AdjustVelocity();

        Jump(verticalSpeed);

        RotateCharacter();

        rb.linearVelocity += gravity * Time.deltaTime;

    }
    void Update()
    {
        IsGrounded = CheckIsGrounded();


        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);


        if (playerInputSpace)
        {
            rightAxis = Vector3.ProjectOnPlane(playerInputSpace.right, upAxis);
            forwardAxis = Vector3.ProjectOnPlane(playerInputSpace.forward, upAxis);
        }
        else
        {
            rightAxis = Vector3.ProjectOnPlane(Vector3.right, upAxis);
            forwardAxis = Vector3.ProjectOnPlane(Vector3.forward, upAxis);
        }

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * movementSpeed;

        desiredJump |= Input.GetButtonDown("Jump");

        Debug.DrawLine(transform.position, transform.position + upAxis * 5, Color.yellow);
        Debug.DrawRay(transform.position + upAxis * 1, moveDirection * 5, Color.red);

        // DoDebug();
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = Vector3.ProjectOnPlane(rightAxis, upAxis).normalized;
        Vector3 zAxis = Vector3.ProjectOnPlane(forwardAxis, upAxis).normalized;

        Debug.DrawRay(transform.position, xAxis * 5, Color.red);
        Debug.DrawRay(transform.position, zAxis * 5, Color.blue);


        float currentX = Vector3.Dot(rb.linearVelocity, xAxis);
        float currentZ = Vector3.Dot(rb.linearVelocity, zAxis);

        bool isDecelerating = desiredVelocity.magnitude < rb.linearVelocity.magnitude;
        float acceleration = isDecelerating ? (IsGrounded ? maxDeceleration : maxAirDeceleration) :
            (IsGrounded ? maxAcceleration : maxAirAcceleration);
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        rb.linearVelocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

    }

    private bool CheckIsGrounded()
    {
        Vector3 castFrom = transform.position + transform.up * 1f;
        bool didHit = Physics.SphereCast(castFrom, 0.5f, -transform.up, out RaycastHit hit, groundDetectionDistance + 0.5f, groundDetectionLayer);
        return didHit;
    }

    void Jump(float verticalSpeed)
    {
        if (Input.GetButton("Jump"))
        {

            if (p.numJumps > 0 && desiredJump)
            { // Jumping from ground
                rb.linearVelocity -= verticalSpeed * upAxis; // Reset vertical velocity of player
                rb.AddForce(upAxis * jumpForce, ForceMode.VelocityChange);
                p.numJumps--;
                p.timeSinceJump = 0;
                desiredJump = false;
            }
        }
        else
        {
            if (IsGrounded)
            {
                p.numJumps = playerValues.numJumps;
                p.timeSinceJump = 0;
            }
            if (!IsGrounded && verticalSpeed > 0)
            {
                // Add extra acceleration down
                rb.AddForce(-upAxis * jumpExtraAcceleration, ForceMode.Acceleration);
            }
        }
        if (!IsGrounded) p.timeSinceJump += Time.fixedDeltaTime;
    }

    void RotateCharacter()
    {
        if (desiredVelocity.magnitude != 0 && rb.linearVelocity.magnitude > 0.001f)
        {
            float rotationSpeed = IsGrounded ? groundedRotationSpeed : airRotationSpeed;
            Quaternion targetRotationMove = Quaternion.LookRotation(forwardAxis, upAxis) * Quaternion.LookRotation(desiredVelocity, Vector3.up);
            Quaternion currentRotationNormal = Quaternion.Slerp(transform.rotation, targetRotationMove, rotationSpeed * Time.deltaTime);

            rb.MoveRotation(currentRotationNormal);
        }
    }
}
