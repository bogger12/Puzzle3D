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

    private float playerGravity;

    // Input Component
    private PlayerInputs playerInputs;

    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        base.HeldBy(holdingBody, holdAnchor);
        PlayerMovement playerMovement = holdingBody.GetComponent<PlayerMovement>();
        playerGravity = playerMovement.gravityAcceleration;
        initialPlayerAirAcceleration = playerMovement.maxAirAcceleration;
        initialPlayerAirDeceleration = playerMovement.maxAirDeceleration;
        initialAirRotationSpeed = playerMovement.airRotationSpeed;

        playerMovement.maxAirAcceleration *= playerAirAccelerationMult;
        playerMovement.maxAirDeceleration *= playerAirDecelerationMult;
        playerMovement.airRotationSpeed *= playerAirRotationSpeedMult;
        playerInputs = holdingBody.GetComponent<PlayerInputs>();
    }

    public override void OnThrow()
    {
        PlayerMovement playerMovement = holdingBody.GetComponent<PlayerMovement>();
        playerMovement.maxAirAcceleration = initialPlayerAirAcceleration;
        playerMovement.maxAirDeceleration = initialPlayerAirDeceleration;
        playerMovement.airRotationSpeed = initialAirRotationSpeed;
        playerInputs = null;
        base.OnThrow();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (holdingBody != null && (!onlyGlideWhileButtonPress || playerInputs.jump.IsPressed()))
        {
            if (boostDuringJumpUp || holdingBody.linearVelocity.y < 0)
            {
                holdingBody.AddForce(playerGravity * playerUpwardsGravityMult * Vector3.up, ForceMode.Acceleration);
                // Limit downward velocity
                if (playerMaxDownwardVelocity != 0)
                {
                    float newVelocityY = Mathf.Max(holdingBody.linearVelocity.y, -playerMaxDownwardVelocity);
                    holdingBody.linearVelocity = new Vector3(holdingBody.linearVelocity.x, newVelocityY, holdingBody.linearVelocity.z);
                }
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
