using UnityEngine;

[RequireComponent(typeof(SpawnParticlesOnDestroy))]
[RequireComponent(typeof(Animator))]
public class CoinCollect : MonoBehaviour
{

    private SpawnParticlesOnDestroy coinParticles;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinParticles = GetComponent<SpawnParticlesOnDestroy>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool finishedCoinShrinkAnimation = stateInfo.IsName("CoinShrink") && stateInfo.normalizedTime >= 1f && !stateInfo.loop;
        if (finishedCoinShrinkAnimation)
        {
            coinParticles.DestroyAndSpawnParticles();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCoinCollector>(out PlayerCoinCollector collector))
        {
            collector.AddCoin();
            animator.enabled = true; // Activate shrink animation
        }
    }
}
