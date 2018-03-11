using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTests : MonoBehaviour
{
    Panel panel;
    const string Show = "Show";
    const string Hide = "Hide";
    const string Center = "Center";
    void Start()
    {
        panel = GetComponent<Panel>();
        Panel.Position centerPos = new Panel.Position(Center, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter);
        panel.AddPosition(centerPos);
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), Show))
            panel.SetPosition(Show, true);
        if (GUI.Button(new Rect(10, 50, 100, 30), Hide))
            panel.SetPosition(Hide, true);
        if (GUI.Button(new Rect(10, 90, 100, 30), Center))
        {
            Tweener t = panel.SetPosition(Center, true);
            t.easingControl.equation = EasingEquations.EaseInOutBack;
        }
    }
}
