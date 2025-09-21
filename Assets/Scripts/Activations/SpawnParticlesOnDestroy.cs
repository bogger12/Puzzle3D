using UnityEngine;

public class SpawnParticlesOnDestroy : MonoBehaviour
{

    public GameObject particles;
    public virtual GameObject DestroyAndSpawnParticles()
    {
        GameObject breakParticles = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
        return breakParticles;
    }
}
