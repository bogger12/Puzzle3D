using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoldableFlower : Holdable
{
    [Header("Flower Glider Effects")]
    public float playerUpwardsGravityMult = 0.5f;
    public float selfUpwardsGravityMult = 0.5f;

    public float playerMaxDownwardVelocity = 0.2f;
    public float selfMaxDownwardVelocity = 0.2f;

    public bool boostDuringJumpUp = false;
    public bool onlyGlideWhileButtonPress = false;

    [Header("Player Air Acceleration")]
    public float playerAirAccelerationMult = 1;
    public float playerAirDecelerationMult = 1;
    public float playerAirRotationSpeedMult = 1;

    private float initialPlayerAirAcceleration;
    private float initialPlayerAirDeceleration;
    private float initialAirRotationSpeed;


    [Header("Rotation In Air")]
    public Transform rotateAroundAnchor;
    public float facingRotationSpeed;
    public float rollRotationSpeed;
    public float rotationMult;

    private float playerGravity;

    // Input Component
    private PlayerInputStore playerInputStore;
    private PlayerMovement playerMovement;

    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        base.HeldBy(holdingBody, holdAnchor);
        playerMovement = holdingBody.GetComponent<PlayerMovement>();
        playerGravity = playerMovement.gravityAcceleration;
        initialPlayerAirAcceleration = playerMovement.maxAirAcceleration;
        initialPlayerAirDeceleration = playerMovement.maxAirDeceleration;
        initialAirRotationSpeed = playerMovement.airRotationSpeed;

        playerMovement.maxAirAcceleration *= playerAirAccelerationMult;
        playerMovement.maxAirDeceleration *= playerAirDecelerationMult;
        playerMovement.airRotationSpeed *= playerAirRotationSpeedMult;
        playerInputStore = holdingBody.GetComponent<PlayerInputStore>();
    }

    public override void OnThrow(float physicsIgnoreTime)
    {
        playerMovement.maxAirAcceleration = initialPlayerAirAcceleration;
        playerMovement.maxAirDeceleration = initialPlayerAirDeceleration;
        playerMovement.airRotationSpeed = initialAirRotationSpeed;
        playerInputStore = null;
        base.OnThrow(physicsIgnoreTime);
        playerMovement = null;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (HoldingBody != null && (!onlyGlideWhileButtonPress || playerInputStore.playerInput.actions["Jump"].IsPressed()))
        {
            if (boostDuringJumpUp || HoldingBody.linearVelocity.y < 0)
            {
                HoldingBody.AddForce(playerGravity * playerUpwardsGravityMult * Vector3.up, ForceMode.Acceleration);
                // Limit downward velocity
                if (playerMaxDownwardVelocity != 0)
                {
                    float newVelocityY = Mathf.Max(HoldingBody.linearVelocity.y, -playerMaxDownwardVelocity);
                    HoldingBody.linearVelocity = new Vector3(HoldingBody.linearVelocity.x, newVelocityY, HoldingBody.linearVelocity.z);
                }
                // playerMovement.RotatePlayerFromFlower(rotationSpeed);
                // RotatePlayer();
            }
        }
        if (rb.useGravity)
        {
            rb.AddForce(9.81f * selfUpwardsGravityMult * Vector3.up, ForceMode.Acceleration);
            if (selfMaxDownwardVelocity != 0)
            {
                float newVelocityY = Mathf.Max(rb.linearVelocity.y, -selfMaxDownwardVelocity);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, newVelocityY, rb.linearVelocity.z);
            }
        }
    }
}
