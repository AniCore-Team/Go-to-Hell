using System.IO;
using UnityEditor;
using UnityEngine;

public class Tools
{
    [MenuItem("Tools/EditorClearSave")]
    public static void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        DirectoryInfo di = new DirectoryInfo(FileManager.FolderPath);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
    }
}