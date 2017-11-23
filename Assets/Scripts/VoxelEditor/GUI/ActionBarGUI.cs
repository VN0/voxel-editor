﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarGUI : GUIPanel
{
    public EditorFile editorFile;

    public override void OnGUI()
    {
        base.OnGUI();

        panelRect = new Rect(190, 10, scaledScreenWidth - 190, 20);

        if (GUI.Button(new Rect(panelRect.xMin, panelRect.yMin, 80, 20), "Play"))
        {

            editorFile.LoadScene("playScene");
        }

        if (GUI.Button(new Rect(panelRect.xMin + 90, panelRect.yMin, 80, 20), "Close"))
        {
            editorFile.LoadScene("menuScene");
        }
    }
}