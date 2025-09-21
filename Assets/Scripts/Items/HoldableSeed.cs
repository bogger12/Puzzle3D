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
        bool hasInteractableLayer = (LayerMask.NameToLayer("Interactable") & collision.collider.gameObject.layer) != 0;
        bool hasPlayerLayer = (LayerMask.NameToLayer("Player") & collision.collider.gameObject.layer) != 0;
        
        if (seedActive && !hasInteractableLayer && !hasPlayerLayer)
        {
            seedActive = false;
            ContactPoint contact = collision.GetContact(0);
            Vector3 collisionNormal = contact.normal; // Dunno if i should get first one
            Debug.DrawRay(contact.point, collisionNormal * 5f, Color.red, 5f);
            spawnOnHit.Spawn(collision.transform, contact.point, collisionNormal);
        }
    }
}
