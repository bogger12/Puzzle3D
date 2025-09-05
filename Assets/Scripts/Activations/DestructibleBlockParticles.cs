using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestructibleBlockParticles : MonoBehaviour
{

    public float destroyAfterSeconds = 1;
    private float timeAlive;

    void Start()
    {
        GetComponent<ParticleSystem>().Play();
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > destroyAfterSeconds) Destroy(gameObject);
    }

}
