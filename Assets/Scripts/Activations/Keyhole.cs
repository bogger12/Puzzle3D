using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Keyhole : MonoBehaviour
{

    public Transform keyAnchor;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            HoldableKey holdableKey = other.GetComponent<HoldableKey>();
            holdableKey.PutKeyInHole(keyAnchor);
        }
    }
}
