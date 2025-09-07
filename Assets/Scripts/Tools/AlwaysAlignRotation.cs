using UnityEngine;

public class AlwaysAlignRotation : MonoBehaviour
{
    public Quaternion alignRotation = Quaternion.identity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = alignRotation;
    }
}
