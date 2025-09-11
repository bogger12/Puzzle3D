using UnityEngine;

[RequireComponent(typeof(SeedSpawnOnHit))]
public class HoldableSeed : Holdable
{

    private bool seedActive = false;
    private SeedSpawnOnHit spawnOnHit;

    protected override void Start()
    {
        base.Start();
        spawnOnHit = GetComponent<SeedSpawnOnHit>();
    }
    public override void HeldBy(Rigidbody holdingBody, Transform holdAnchor)
    {
        base.HeldBy(holdingBody, holdAnchor);
    }

    public override void OnThrow(float physicsIgnoreTime)
    {
        seedActive = true;
        base.OnThrow(physicsIgnoreTime);
    }



    void OnCollisionEnter(Collision collision)
    {
        if (seedActive)
        {
            seedActive = false;
            ContactPoint contact = collision.GetContact(0);
            Vector3 collisionNormal = contact.normal; // Dunno if i should get first one
            Debug.DrawRay(contact.point, collisionNormal * 5f, Color.red, 5f);
            spawnOnHit.Spawn(contact.point, collisionNormal);
        }
    }
}
