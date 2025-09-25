using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(RemoteActivate))]
public class LeverMeasure : MonoBehaviour
{
    private RemoteActivate remoteActivate;
    private HingeJoint hinge;

    private bool lastStatus = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        remoteActivate = GetComponent<RemoteActivate>();
        hinge = GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        bool currentStatus = hinge.angle > 0;
        // VisualVarDisplay.SetDebugBool("lever", currentStatus);
        if (currentStatus != lastStatus) remoteActivate.SetActive(currentStatus);
        lastStatus = currentStatus;
    }
}
