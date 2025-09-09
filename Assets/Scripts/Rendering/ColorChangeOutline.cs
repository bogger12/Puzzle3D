using System.Collections.Generic;
using UnityEngine;

public class ColorChangeOutline : Outline
{

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // protected override void Start()
    // {
    //     base.Start();
    // }

    // protected override void SetOutline(bool isenabled)
    // {
    //     base.SetOutline(isenabled);
    // }

    protected virtual void ChangeColor(Color color)
    {
        if (Enabled) outlineMaterial.color = color;
    }

}
