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

                explosionMesh.material.SetFloat("_ExplodeProgress", sizeOverExplosionTime.Evaluate(timePassed));
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

}