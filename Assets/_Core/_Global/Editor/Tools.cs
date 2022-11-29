using System.IO;
using System.Linq;
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
        foreach (var dir in di.GetDirectories())
        {
            if (dir.Name == $"{1}Slot")
            {
                var fileNames = dir.GetFiles();
                foreach (var file in fileNames)
                {
                    File.Delete(file.FullName);
                }
                string fullPath = FileManager.FolderPath + $"{1}Slot";
                Directory.Delete(fullPath);
                if (di.GetFiles().Any(f => f.Name == $"{1}Slot.meta"))
                    File.Delete(fullPath + ".meta");
            }
        }
    }
}