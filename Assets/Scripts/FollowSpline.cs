using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class FollowSpline : Activateable
{

    private SplineAnimate splineAnimate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void SetActive(bool active)
    {
        if (active) splineAnimate.Play();
        else splineAnimate.Pause();
    }
}
