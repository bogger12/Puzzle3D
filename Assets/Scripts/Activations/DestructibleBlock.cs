using UnityEngine;

public class DestructibleBlock : MonoBehaviour
{

    public GameObject particles;
    public float speedToSet = 10;

    public void DestroyBlockFrom(Vector3 bombPosition)
    {
        GameObject blockParticles = Instantiate(particles, transform.position, Quaternion.identity);
        ParticleSystem destructibleBlockParticles = blockParticles.GetComponent<ParticleSystem>();
        var shape = destructibleBlockParticles.shape;
        shape.rotation = Quaternion.LookRotation(Vector3.Normalize(transform.position - bombPosition)).eulerAngles;
        var main = destructibleBlockParticles.main;
        main.startSpeed = speedToSet;
        Destroy(gameObject);
    }
}
