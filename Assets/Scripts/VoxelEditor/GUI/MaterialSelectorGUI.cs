﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MaterialSelectorGUI : GUIPanel
{
    public VoxelArray voxelArray;
    public bool overlay;

    List<string> materialNames;
    List<Texture> materialPreviews;
    string materialDirectory = "GameAssets/Materials";
    List<string> materialSubDirectories;

    public override void OnEnable()
    {
        depth = -1;
        base.OnEnable();
    }

    void Start()
    {
        if (overlay)
        {
            materialDirectory = "GameAssets/Overlays";
        }
        else
        {
            materialDirectory = "GameAssets/Materials";
        }
        UpdateMaterialDirectory();
    }

    public override void OnGUI()
    {
        base.OnGUI();

        panelRect = new Rect(scaledScreenWidth - 180, 0, 180, PropertiesGUI.targetHeight);

        GUI.Box(panelRect, "Assign Material");

        if (materialPreviews == null)
            return;
        Rect scrollBox = new Rect(panelRect.xMin, panelRect.yMin + 25, panelRect.width, panelRect.height - 25);
        float scrollAreaWidth = panelRect.width - 1;
        float buttonWidth = scrollAreaWidth - 20;
        float scrollAreaHeight = materialSubDirectories.Count * 25 + materialPreviews.Count * buttonWidth;
        Rect scrollArea = new Rect(0, 0, scrollAreaWidth, scrollAreaHeight);
        scroll = GUI.BeginScrollView(scrollBox, scroll, scrollArea);
        float y = 0;
        for (int i = 0; i < materialSubDirectories.Count; i++)
        {
            string subDir = materialSubDirectories[i];
            if (GUI.Button(new Rect(10, y, buttonWidth, 20), subDir))
            {
                scroll = new Vector2(0, 0);
                MaterialDirectorySelected(materialSubDirectories[i]);
            }
            y += 25;
        }
        for (int i = 0; i < materialPreviews.Count; i++)
        {
            Texture materialPreview = materialPreviews[i];
            Rect buttonRect = new Rect(10, y, buttonWidth, buttonWidth);
            Rect textureRect = new Rect(buttonRect.xMin + 10, buttonRect.yMin + 10,
                buttonRect.width - 20, buttonRect.height - 20);
            if (GUI.Button(buttonRect, ""))
            {
                MaterialSelected(materialNames[i]);
            }
            GUI.DrawTexture(textureRect, materialPreview, ScaleMode.ScaleToFit);
            y += buttonWidth;
        }
        GUI.EndScrollView();
    }

    void UpdateMaterialDirectory()
    {
        materialSubDirectories = new List<string>();
        materialSubDirectories.Add("..");
        materialNames = new List<string>();
        materialPreviews = new List<Texture>();
        foreach (string dirEntry in ResourcesDirectory.dirList)
        {
            if (dirEntry.Length <= 2)
                continue;
            string newDirEntry = dirEntry.Substring(2);
            string directory = Path.GetDirectoryName(newDirEntry);
            if (directory != materialDirectory)
                continue;
            string extension = Path.GetExtension(newDirEntry);
            if (extension == "")
                materialSubDirectories.Add(Path.GetFileName(newDirEntry));
            else if (extension == ".mat")
            {
                materialNames.Add(Path.GetFileNameWithoutExtension(newDirEntry));
                Material material = Resources.Load<Material>(directory + "/" + Path.GetFileNameWithoutExtension(newDirEntry));
                if (material == null)
                {
                    materialPreviews.Add(null);
                    continue;
                }

                Texture previewTexture = null;
                Color color = Color.white;

                if (material.mainTexture != null)
                    previewTexture = material.mainTexture;
                else if (material.HasProperty("_Color"))
                    color = material.color;
                else if (material.HasProperty("_ColorControl"))
                    // water shader
                    previewTexture = material.GetTexture("_ColorControl");
                if (previewTexture == null)
                {
                    Texture2D solidColorTexture = new Texture2D(1, 1);
                    solidColorTexture.SetPixel(0, 0, color);
                    solidColorTexture.Apply();
                    previewTexture = solidColorTexture;
                }
                materialPreviews.Add(previewTexture);
            }
        }

        Resources.UnloadUnusedAssets();
    }

    void MaterialDirectorySelected(string name)
    {
        if (name == "..")
        {
            if (materialDirectory.Trim() != "")
                materialDirectory = Path.GetDirectoryName(materialDirectory);
            UpdateMaterialDirectory();
            return;
        }
        else
        {
            if (materialDirectory.Trim() == "")
                materialDirectory = name;
            else
                materialDirectory += "/" + name;
            UpdateMaterialDirectory();
        }
    }

    void MaterialSelected(string name)
    {
        string materialPath = materialDirectory + "/" + name;
        Material material = Resources.Load<Material>(materialPath);
        voxelArray.AssignMaterial(material, overlay);
        Destroy(this);
    }
}