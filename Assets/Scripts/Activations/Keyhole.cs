using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Keyhole : MonoBehaviour
{

    public Transform keyAnchor;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Unlock()
    {
        Debug.Log("Unlocked!");
    }

    public void Lock()
    {
        Debug.Log("Locked.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HoldableKey>(out HoldableKey holdableKey))
        {
            holdableKey.SetActiveKeyHole(this);
        }
        // if (other.CompareTag("Key"))
        // {
        //     HoldableKey holdableKey = other.TryGetComponent<HoldableKey>();
        //     if (holdableKey.heldStatus == HoldableStatus.Held)
        //         holdableKey.PutKeyInHole(keyAnchor);
        // }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HoldableKey>(out HoldableKey holdableKey) && holdableKey.GetActiveKeyHole() == this)
        {
            holdableKey.SetActiveKeyHole(null);
        }
    }
}
