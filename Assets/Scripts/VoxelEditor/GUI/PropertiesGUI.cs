﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropertiesGUI : GUIPanel {

    public VoxelArray voxelArray;

    MaterialSelectorGUI materialSelector;

    public override void OnGUI()
    {
        base.OnGUI();

        panelRect = new Rect(0, 0, 180, targetHeight);

        GUI.Box(panelRect, "Properties");

        Rect scrollBox = new Rect(panelRect.xMin, panelRect.yMin + 25, panelRect.width, panelRect.height - 25);
        float scrollAreaWidth = panelRect.width - 1;
        float scrollAreaHeight = scrollBox.height - 1; // TODO
        Rect scrollArea = new Rect(0, 0, scrollAreaWidth, scrollAreaHeight);
        scroll = GUI.BeginScrollView(scrollBox, scroll, scrollArea);

        if (GUI.Button(new Rect(scrollArea.xMin + 10, scrollArea.yMin, scrollArea.width - 20, 20), "Set Material"))
        {
            if (materialSelector == null)
            {
                materialSelector = gameObject.AddComponent<MaterialSelectorGUI>();
                materialSelector.voxelArray = voxelArray;
            }
        }

        if (GUI.Button(new Rect(scrollArea.xMin + 10, scrollArea.yMin + 25, scrollArea.width - 70, 20), "Set Overlay"))
        {
            if (materialSelector == null)
            {
                materialSelector = gameObject.AddComponent<MaterialSelectorGUI>();
                materialSelector.voxelArray = voxelArray;
                materialSelector.overlay = true;
            }
        }

        if (GUI.Button(new Rect(scrollArea.xMax - 60, scrollArea.yMin + 25, 50, 20), "Clear"))
        {
            voxelArray.AssignMaterial(null, true);
        }

        if (GUI.Button(new Rect(scrollArea.xMin + 10, scrollArea.yMin + 50, (scrollArea.width - 20) / 2, 20), "Left"))
        {
            voxelArray.OrientFaces(1);
        }

        if (GUI.Button(new Rect(scrollArea.xMin + 10 + (scrollArea.width - 20) / 2, scrollArea.yMin + 50, (scrollArea.width - 20) / 2, 20), "Right"))
        {
            voxelArray.OrientFaces(3);
        }

        if (GUI.Button(new Rect(scrollArea.xMin + 10, scrollArea.yMin + 75, (scrollArea.width - 20) / 2, 20), "Flip Horizontal"))
        {
            voxelArray.OrientFaces(5);
        }

        if (GUI.Button(new Rect(scrollArea.xMin + 10 + (scrollArea.width - 20) / 2, scrollArea.yMin + 75, (scrollArea.width - 20) / 2, 20), "Flip Vertical"))
        {
            voxelArray.OrientFaces(7);
        }

        GUI.EndScrollView();
    }

}