using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public static class VisualVarDisplay
{

    public static void SetDebugBool(string label, bool status)
    {
        List<DebugBoolDisplay> boolDisplays = GameObject.FindObjectsByType<DebugBoolDisplay>(FindObjectsSortMode.None).ToList();

        foreach (DebugBoolDisplay display in boolDisplays)
        {
            if (display.label == label) display.activeBool = status;
        }
    }
}