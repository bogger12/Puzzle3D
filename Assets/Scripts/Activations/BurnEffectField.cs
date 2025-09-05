using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BurnEffectField : MonoBehaviour
{

    Collider fieldCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fieldCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Burnable"))
        // {
            if (other.TryGetComponent<Burnable>(out Burnable b))
            {
                Debug.Log("Found and trying to burn " + b.gameObject.name);
                b.StartBurn();
            }
        // }
    }
}
