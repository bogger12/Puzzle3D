using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoldableFlower : Holdable
{
    public float playerUpwardsGravityMult = 0.5f;
    public float selfUpwardsGravityMult = 0.5f;

    public float playerMaxDownwardVelocity = 0.2f;
    public float selfMaxDownwardVelocity = 0.2f;

    public bool boostDuringJumpUp = false;

    private float playerGravity;
    private Rigidbody rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void HeldBy(Rigidbody holdingBody)
    {
        isHeld = true;
        this.holdingBody = holdingBody;
        playerGravity = holdingBody.GetComponent<PlayerMovement>().gravityAcceleration;
        Debug.Log("Player gravity is " + playerGravity);
    }

    public void FixedUpdate()
    {
        if (holdingBody != null)
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
