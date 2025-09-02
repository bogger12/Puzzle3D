using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class LeverMeasure : MonoBehaviour
{

    private HingeJoint hinge;
    private RemoteActivate remoteActivate;

    private bool lastStatus = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        remoteActivate = GetComponent<RemoteActivate>();
    }

    // Update is called once per frame
    void Update()
    {
        bool currentStatus = hinge.angle > 0;
        VisualVarDisplay.SetDebugBool("lever", currentStatus);
        if (currentStatus != lastStatus) remoteActivate.SetActive(currentStatus);
        lastStatus = currentStatus;
    }
}
