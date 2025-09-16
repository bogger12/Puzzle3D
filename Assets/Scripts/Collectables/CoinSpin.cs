using UnityEngine;

public class CoinSpin : MonoBehaviour
{

    public float spinSpeed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotateAngle = spinSpeed * Time.deltaTime;
        transform.Rotate(transform.up, rotateAngle);
    }
}
