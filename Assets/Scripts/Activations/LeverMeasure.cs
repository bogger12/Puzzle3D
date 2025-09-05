using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class LeverMeasure : RemoteActivate
{

    private HingeJoint hinge;

    private bool lastStatus = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        bool currentStatus = hinge.angle > 0;
        VisualVarDisplay.SetDebugBool("lever", currentStatus);
        if (currentStatus != lastStatus) SetActive(currentStatus);
        lastStatus = currentStatus;
    }
}
