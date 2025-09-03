using UnityEngine;


enum ExplosionState
{
    Exploding,
    Fading
}

[RequireComponent(typeof(MeshRenderer))]
public class Explosion : MonoBehaviour
{

    public AnimationCurve sizeOverExplosionTime;
    public AnimationCurve alphaAfterExplosion;
    private MeshRenderer explosionMesh;

    private float timePassed;

    private ExplosionState explosionState = ExplosionState.Exploding;

    private float blastStrength;
    private float explodeRadius;
    private float explodeUpwardsModifier;
    private LayerMask explosionMoveMask;
    private Vector2 fuseTimeRangeOfAffectedBomb;

    Collider[] surroundingBodies;

    void Start()
    {
        explosionMesh = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        switch (explosionState)
        {
            case ExplosionState.Exploding:
                {
                    float endTime = sizeOverExplosionTime[sizeOverExplosionTime.keys.Length - 1].time;
                    if (endTime <= timePassed)
                    {
                        explosionState = ExplosionState.Fading;
                        timePassed = 0;
                        break;
                    }
                    float progress = sizeOverExplosionTime.Evaluate(timePassed);
                    explosionMesh.material.SetFloat("_ExplodeProgress", progress);

                    // Affect Surrounding Rigidbodies

                    if (surroundingBodies==null) surroundingBodies = Physics.OverlapSphere(transform.position, explodeRadius, explosionMoveMask);

                    float currentRadius = explodeRadius * progress;

                    foreach (Collider c in surroundingBodies)
                    {
                        if (c!=null && c.gameObject.TryGetComponent<Rigidbody>(out Rigidbody body))
                        {
                            float bodyDistance = Vector3.Distance(body.position, transform.position);
                            if (bodyDistance <= currentRadius)
                            {
                                body.AddExplosionForce(blastStrength, transform.position, explodeRadius, explodeUpwardsModifier, ForceMode.Impulse);
                                if (c.gameObject.TryGetComponent<DestructibleBlock>(out DestructibleBlock destructibleBlock))
                                {
                                    destructibleBlock.DestroyBlockFrom(transform.position);
                                }
                                if (c.gameObject.TryGetComponent<BombExplode>(out BombExplode bomb))
                                {
                                    bomb.StartFuse(Random.Range(fuseTimeRangeOfAffectedBomb.x, fuseTimeRangeOfAffectedBomb.y));
                                }
                            }
                        }
                    }
                    break;
                }
            case ExplosionState.Fading:
                {
                    float endTime = alphaAfterExplosion[alphaAfterExplosion.keys.Length - 1].time;
                    if (endTime <= timePassed)
                    {
                        GameObject.Destroy(this.gameObject);
                        break;
                    }
                    explosionMesh.material.SetFloat("_TransparencyFade", alphaAfterExplosion.Evaluate(timePassed));
                    break;
                }
        }
    }


    public void InitialiseExplosionVariables(float blastStrength, float explodeRadius, float explodeUpwardsModifier, LayerMask explosionMoveMask, Vector2 fuseTimeRangeOfAffectedBomb)
    {
        this.blastStrength = blastStrength;
        this.explodeRadius = explodeRadius;
        this.explodeUpwardsModifier = explodeUpwardsModifier;
        this.explosionMoveMask = explosionMoveMask;
        this.fuseTimeRangeOfAffectedBomb = fuseTimeRangeOfAffectedBomb;
    }
}