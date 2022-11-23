using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileManager
{
    public static readonly string FolderPath = Application.dataPath + "/GtH/";

    public static bool WriteToFile(string fileName, string fileContents)
    {
        var folders = fileName.Split('/');
        string addFolders = "";
        for (int i = 0; i < folders.Length - 1; i++)
            addFolders += (folders[i] + "/");
        if (!Directory.Exists(FolderPath + addFolders))
        {
            Directory.CreateDirectory(FolderPath + addFolders);
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
