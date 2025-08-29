using UnityEngine;

public class ButtonMeasureStop : MonoBehaviour
{

    public Transform frame;
    public float maxPenetration = 1;

    public float pressedThreshold = 0.1f;
    private Vector3 initialRelativePos;
    private Rigidbody rb;
    private ActivateOnPress activateOnPress;

    private bool lastPressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialRelativePos = transform.position - frame.position;
        rb = GetComponent<Rigidbody>();
        activateOnPress = GetComponent<ActivateOnPress>();
        Debug.Log(initialRelativePos);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yDistance = transform.position.y - (frame.position + initialRelativePos).y;

        if (Mathf.Abs(yDistance) > maxPenetration)
        {
            // Debug.Log("Adding support force");
            rb.MovePosition(frame.position + initialRelativePos + (Vector3.down * maxPenetration));
        }

        // Activate something based on button press (and show debug visual)
        bool currentPressed = Mathf.Abs(yDistance) > pressedThreshold;
        VisualVarDisplay.SetDebugBool("button", currentPressed);
        if (lastPressed != currentPressed) activateOnPress.SetActive(currentPressed);
        lastPressed = currentPressed;
        
    }
}
