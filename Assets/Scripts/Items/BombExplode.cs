using UnityEngine;



enum BombState
{
    Inactive,
    BurningFuse
}

[RequireComponent(typeof(Rigidbody))]
public class BombExplode : MonoBehaviour
{


    public float fuseTime;
    public GameObject explosionEffect;
    [Header("Explosion Affects Surroundings")]
    public float explodeRadius;
    public float blastStrength = 1;
    public float explodeUpwardsModifier = 0;
    public LayerMask explosionMoveMask;



    private float timeUntilExplode = 0;
    private BombState bombState = BombState.Inactive;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (bombState!=BombState.Inactive) timeUntilExplode -= Time.deltaTime;
        switch (bombState)
        {
            case BombState.BurningFuse:
                {
                    if (timeUntilExplode <= 0) Explode();
                    break;
                }
        }
    }


    public void StartFuse()
    {
        timeUntilExplode = fuseTime;
        bombState = BombState.BurningFuse;
        Debug.Log("Fuse Started...");
    }

    private void Explode()
    {
        Debug.Log("Explode!");
        GameObject explosion = GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity);
        MeshRenderer explosionMesh = explosion.GetComponent<MeshRenderer>();
        explosionMesh.material.SetFloat("_ExplodeProgress", 0);
        explosionMesh.material.SetFloat("_TransparencyFade", 1);
        explosionMesh.material.SetFloat("_MaxSize", explodeRadius);

        explosion.GetComponent<Explosion>().InitialiseExplosionVariables(blastStrength, explodeRadius, explodeUpwardsModifier, explosionMoveMask);

        GameObject.Destroy(this.gameObject);
    }
}
