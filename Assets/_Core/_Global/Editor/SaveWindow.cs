using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class SaveWindow : EditorWindow
{
    private static string[] saves_id = new string[1]
    {
        "homelessonprojectdata.sav"
    };

    [MenuItem("Tools/Reset Saves")]
    public static void Show()
    {
        //GetWindow<HierarchyMarker>("Create Hierarchy Marker");
        ResetSaves();
    }

    public static void ResetSaves()
    {
        foreach (var id in saves_id)
        {
            if (File.Exists(GetFilePath(id)))
            {
                File.Delete(GetFilePath(id));
                Debug.Log($"Delete {id}");
            }
        }
        
        PlayerPrefs.DeleteAll();
    }

    public static string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
