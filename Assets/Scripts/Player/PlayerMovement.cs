using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(PlayerThrow))]
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

    public Rigidbody rb;
    public bool IsGrounded { get; private set; }
    private Vector3 moveDirection;

    private Vector3 groundNormal;
    public float maxGroundAngle = 45;
    public bool useNormalOfGround = true;
    [HideInInspector]
    public Vector3 upAxis, rightAxis, forwardAxis;
    [HideInInspector]
    public Vector3 desiredVelocity;
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
    private PlayerThrow playerThrow;

    private Vector3 xAxis, zAxis;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInputs = GetComponent<PlayerInputs>();
        playerThrow = GetComponent<PlayerThrow>();
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

        xAxis = Vector3.ProjectOnPlane(movementRightAxis, upAxis).normalized;
        zAxis = Vector3.ProjectOnPlane(movementForwardAxis, upAxis).normalized;



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
        // Debug.Log("accumulatedVelocity = " + accumulatedVelocity);

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
        Quaternion newRotation = rb.rotation;
        if (!IsGrounded && playerThrow.HeldBody != null && playerThrow.HeldBody.TryGetComponent<HoldableFlower>(out HoldableFlower flower))
        {
            newRotation = RotatePlayerFromFlower(flower.rotationTargetAngle, flower.rotationMaxAngle, flower.rollRotationSpeed, flower.facingRotationSpeed);
        }
        else if (desiredVelocity.magnitude != 0 && rb.linearVelocity.magnitude > 0.001f)
        {
            float rotationSpeed = IsGrounded ? groundedRotationSpeed : airRotationSpeed;
            Quaternion targetRotationMove = Quaternion.LookRotation(forwardAxis, upAxis) * Quaternion.LookRotation(desiredVelocity, Vector3.up);
            newRotation = Quaternion.Slerp(transform.rotation, targetRotationMove, rotationSpeed * Time.deltaTime);
        }
        rb.MoveRotation(newRotation);
    }

    Vector3 currentDesiredVelocity = Vector3.zero;
    Vector3 currentHorizontalVelocity = Vector3.zero;
    public Quaternion RotatePlayerFromFlower(float rotationTargetAngle, float rotationMaxAngle, float rollRotationSpeed, float facingRotationSpeed)
    {
        // Get difference between current velocity and target velocity direction

        if (desiredVelocity.magnitude > 0.001) currentDesiredVelocity = desiredVelocity;

        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, upAxis);
        if (horizontalVelocity.magnitude > 0.001) currentHorizontalVelocity = horizontalVelocity;

        Debug.DrawRay(transform.position, currentHorizontalVelocity * 10, Color.red);

        Vector3 rotationForward = Vector3.ProjectOnPlane(rb.rotation * Vector3.forward, upAxis);
        Debug.DrawRay(transform.position, rotationForward * 10, Color.magenta);

        Vector3 rotationRight = Quaternion.AngleAxis(-90, Vector3.up) * rotationForward;
        Debug.DrawRay(transform.position, rotationForward * 10, Color.blue);

        float directionDifference = Vector3.Dot(rotationRight.normalized, currentHorizontalVelocity.normalized);
        // if (directionDifference < 0.1) directionDifference = 0;
        directionDifference = Mathf.Clamp(directionDifference*rotationTargetAngle, -rotationMaxAngle, rotationMaxAngle);
        Debug.Log(directionDifference);

        Quaternion facingRotation = Quaternion.LookRotation(forwardAxis, upAxis) * Quaternion.LookRotation(currentDesiredVelocity, Vector3.up);
        Quaternion rollRotation = Quaternion.LookRotation(currentHorizontalVelocity.normalized, Vector3.up) * Quaternion.AngleAxis(directionDifference, Vector3.forward);

        Quaternion currentRotationNormal = Quaternion.Slerp(transform.rotation, facingRotation, facingRotationSpeed * Time.deltaTime);
        currentRotationNormal = Quaternion.Slerp(currentRotationNormal, rollRotation, rollRotationSpeed * Time.deltaTime);
        // currentRotationNormal *= rollRotation;


        return currentRotationNormal;
    }


    public void AddVelocity(Vector3 vel)
    {
        accumulatedVelocity += vel;
    }
}
