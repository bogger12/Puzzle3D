using UnityEngine;



enum BombState
{
    Inactive,
    BurningFuse
}

[RequireComponent(typeof(Rigidbody))]
public class BombExplode : MonoBehaviour
{

    [Header("Bomb Variables")]
    public float fuseTime;
    public GameObject explosionEffect;
    [Header("Explosion Affects Surroundings")]
    public float explodeRadius;
    public float blastStrength = 1;
    public float explodeUpwardsModifier = 0;
    public LayerMask explosionMoveMask;
    public Vector2 fuseTimeRangeOfAffectedBomb = new Vector2(0.05f, 0.2f);



    private float timeUntilExplode = 0;
    private BombState bombState = BombState.Inactive;

    private MeshRenderer bombMesh;

    void Start()
    {
        bombMesh = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (bombState!=BombState.Inactive) timeUntilExplode -= Time.deltaTime;
        switch (bombState)
        {
            case BombState.BurningFuse:
                {
                    float fuseProgress = 1 - (timeUntilExplode / fuseTime);
                    bombMesh.materials[1].SetFloat("_FuseProgress", fuseProgress);
                    if (timeUntilExplode <= 0) Explode();
                    break;
                }
        }
    }


    public void StartFuse(float time = 0)
    {
        if (bombState == BombState.BurningFuse) return;
        timeUntilExplode = time!=0 ? time : fuseTime ;
        bombState = BombState.BurningFuse;
        Debug.Log("Fuse Started...");
    }

    public void Explode()
    {
        bombState = BombState.Inactive;
        Debug.Log("Explode!");
        GameObject explosion = GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity);
        MeshRenderer explosionMesh = explosion.GetComponent<MeshRenderer>();
        explosionMesh.material.SetFloat("_ExplodeProgress", 0);
        explosionMesh.material.SetFloat("_TransparencyFade", 1);
        explosionMesh.material.SetFloat("_MaxSize", explodeRadius);

        explosion.GetComponent<Explosion>().InitialiseExplosionVariables(blastStrength, explodeRadius, explodeUpwardsModifier, explosionMoveMask, fuseTimeRangeOfAffectedBomb);

        GameObject.Destroy(this.gameObject);
    }
}
