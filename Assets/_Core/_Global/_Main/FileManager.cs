using System;
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static readonly string FolderPath = Application.dataPath + "/GtH/";

    public static bool WriteToFile(string fileName, string fileContents)
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        var fullPath = Path.Combine(FolderPath, fileName);
        if (!File.Exists(fullPath))
            File.Create(fullPath).Close();

        try
        {
            File.WriteAllText(fullPath, fileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    }

    public static bool LoadFromFile(string fileName, out string result)
    {
        result = "";
        string fullPath = Path.Combine(FolderPath, fileName);
        if (!File.Exists(fullPath))
            return false;
        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            return false;
        }
    }
}
