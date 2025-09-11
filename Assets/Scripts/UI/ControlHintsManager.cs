using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlHintsManager : MonoBehaviour
{
    public List<RectTransform> hintAnchors;

    public GameObject controlUIPrefab;
    private Dictionary<RectTransform, ControlAnchorOnPoint> controlAnchors = new();
    void Start()
    {
        foreach (RectTransform rt in hintAnchors)
        {
            AddControlUI(rt);
            rt.GetComponent<Image>().enabled = false;
        }
    }

    void AddControlUI(RectTransform rt)
    {
        GameObject newControlUI = GameObject.Instantiate(controlUIPrefab, rt);
        RectTransform rectTransform = newControlUI.GetComponent<RectTransform>();
        ControlAnchorOnPoint anchorOnPoint = newControlUI.GetComponent<ControlAnchorOnPoint>();
        controlAnchors.Add(rt, anchorOnPoint);
        anchorOnPoint.anchor = rt;
        rectTransform.anchoredPosition = Vector3.zero;
        newControlUI.SetActive(false);
    }

    public void AssignHint(string buttonText, string hintText, bool longPress)
    {
        Debug.Log("Assigning hint of " + hintText);
        foreach (RectTransform rt in hintAnchors)
        {
            if (!rt.GetChild(0).gameObject.activeInHierarchy)
            {
                Debug.Log("Successfully Assigned " + hintText);
                controlAnchors[rt].gameObject.SetActive(true);
                controlAnchors[rt].SetButtonText(buttonText);
                controlAnchors[rt].SetHintText(hintText);
                controlAnchors[rt].longPressIcon.SetActive(longPress);
                Debug.Log(longPress + " " + controlAnchors[rt].longPressIcon.name + " is " + controlAnchors[rt].longPressIcon.activeInHierarchy);
                return;
            }
        }
    }

    public void ResetHints()
    {
        foreach (RectTransform hintRT in hintAnchors)
        {
            hintRT.GetChild(0).gameObject.SetActive(false);
        }
    }
}