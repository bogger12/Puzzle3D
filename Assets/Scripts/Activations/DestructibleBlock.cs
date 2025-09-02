using UnityEngine;

public class DestructibleBlock : MonoBehaviour
{

    public GameObject particles;

    public void DestroyBlockFrom(Vector3 bombPosition)
    {
        GameObject blockParticles = Instantiate(particles, transform.position, Quaternion.identity);
        ParticleSystem destructibleBlockParticles = blockParticles.GetComponent<ParticleSystem>();
        var shape = destructibleBlockParticles.shape;
        shape.rotation = Quaternion.LookRotation(Vector3.Normalize(transform.position - bombPosition)).eulerAngles;
        Destroy(gameObject);
    }
}
