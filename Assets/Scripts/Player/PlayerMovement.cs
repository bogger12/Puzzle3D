using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
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

    private Vector3 groundNormal;
    public float maxGroundAngle = 45;
    public bool useNormalOfGround = true;
    Vector3 upAxis, rightAxis, forwardAxis;
    Vector3 desiredVelocity;
    Vector3 accumulatedVelocity = Vector3.zero;
    [Range(0f, 1f)]
    public float accumulatedVelocityDragMult = 0.9f;
    bool desiredJump;

    [Header("Player Values")]
    public float numJumps = 1;

    private float timeSinceJump = 0f;
    private float currentJumps;

    // Input Component
    private PlayerInputs playerInputs;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInputs = GetComponent<PlayerInputs>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        currentJumps = numJumps;
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


        Vector2 playerInput = playerInputs.move.action.ReadValue<Vector2>();
        // playerInput.x = Input.GetAxis("Horizontal");
        // playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        Vector3 movementUpAxis = useNormalOfGround ? groundNormal : Vector3.up;        

        rightAxis = Vector3.ProjectOnPlane(playerInputSpace ? playerInputSpace.right : Vector3.right, upAxis);
        forwardAxis = Vector3.ProjectOnPlane(playerInputSpace ? playerInputSpace.forward : Vector3.forward, upAxis);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * movementSpeed;

        desiredJump |= playerInputs.jump.action.WasPressedThisFrame();

        Debug.DrawLine(transform.position, transform.position + movementUpAxis * 5, Color.yellow);
        Debug.DrawRay(transform.position + movementUpAxis * 1, moveDirection * 5, Color.red);

        // DoDebug();
    }

    void AdjustVelocity()
    {

        Vector3 movementUpAxis = useNormalOfGround ? groundNormal : Vector3.up;

        Vector3 movementRightAxis = rightAxis;
        Vector3 movementForwardAxis = forwardAxis;

        if (useNormalOfGround) {
            movementRightAxis = Vector3.ProjectOnPlane(playerInputSpace ? playerInputSpace.right : Vector3.right, movementUpAxis);
            movementForwardAxis = Vector3.ProjectOnPlane(playerInputSpace ? playerInputSpace.forward : Vector3.forward, movementUpAxis);
        }

        Vector3 xAxis = Vector3.ProjectOnPlane(movementRightAxis, upAxis).normalized;
        Vector3 zAxis = Vector3.ProjectOnPlane(movementForwardAxis, upAxis).normalized;



        Debug.DrawRay(transform.position, xAxis * 5, Color.red);
        Debug.DrawRay(transform.position, zAxis * 5, Color.blue);


        float currentX = Vector3.Dot(rb.linearVelocity, xAxis);
        float currentZ = Vector3.Dot(rb.linearVelocity, zAxis);

        bool isDecelerating = (desiredVelocity + accumulatedVelocity).magnitude < rb.linearVelocity.magnitude;
        float acceleration = isDecelerating ? (IsGrounded ? maxDeceleration : maxAirDeceleration) :
            (IsGrounded ? maxAcceleration : maxAirAcceleration);
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
        // desiredVelocity = Vector3.zero;


        accumulatedVelocity *= accumulatedVelocityDragMult;
        accumulatedVelocity = Vector3.ClampMagnitude(accumulatedVelocity, acceleration);
        Debug.Log("accumulatedVelocity = " + accumulatedVelocity);

        rb.linearVelocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ) + accumulatedVelocity;



    }

    private bool CheckIsGrounded()
    {
        Vector3 castFrom = transform.position + transform.up * 1f;
        bool didHit = Physics.SphereCast(castFrom, 0.5f, -transform.up, out RaycastHit hit, groundDetectionDistance + 0.5f, groundDetectionLayer);
        if (Vector3.Angle(groundNormal, Vector3.up) >= maxGroundAngle) groundNormal = hit.normal;
        return didHit;
    }

    void Jump(float verticalSpeed)
    {
        if (playerInputs.jump.action.IsPressed())
        {

            if (currentJumps > 0 && desiredJump)
            { // Jumping from ground
                rb.linearVelocity -= verticalSpeed * upAxis; // Reset vertical velocity of player
                rb.AddForce(upAxis * jumpForce, ForceMode.VelocityChange);
                currentJumps--;
                timeSinceJump = 0;
                desiredJump = false;
            }
        }
        else
        {
            if (IsGrounded)
            {
                currentJumps = numJumps;
                timeSinceJump = 0;
            }
            if (!IsGrounded && verticalSpeed > 0)
            {
                // Add extra acceleration down
                rb.AddForce(-upAxis * jumpExtraAcceleration, ForceMode.Acceleration);
            }
        }
        if (!IsGrounded) timeSinceJump += Time.fixedDeltaTime;
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


    public void AddVelocity(Vector3 vel)
    {
        accumulatedVelocity += vel;
    }
}
