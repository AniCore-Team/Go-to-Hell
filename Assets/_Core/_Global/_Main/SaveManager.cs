using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SaveManager
{
    [Inject] private Client client;
    private IMG2Sprite iMG2Sprite = new();
    private Dictionary<int, SaveSlotData> saveSlotDatas = new();
    private int currentSlot;

    public int CurrentSlot { set => currentSlot = value; }

    public void Save()
    {
        if (!saveSlotDatas.ContainsKey(currentSlot))
            saveSlotDatas.Add(currentSlot, new SaveSlotData());

        saveSlotDatas[currentSlot].id = currentSlot;
        saveSlotDatas[currentSlot].clientModel = client.GetClientModel();
        saveSlotDatas[currentSlot].levelModel = LevelController.GetLevelModel();

        FileManager.WriteToFile($"{currentSlot}Slot/ClientSlot.sav", client.GetJsonClientModel());
        FileManager.WriteToFile($"{currentSlot}Slot/LevelSlot.sav", LevelController.GetJsonLevelModel());

        FileManager.WriteToFile($"{currentSlot}Slot/SaveData.sav", JsonConvert.SerializeObject(saveSlotDatas, Formatting.Indented));

        SaveTexture(LevelController.GetScreenshot());
    }

    public void Load()
    {
        DirectoryInfo di = new DirectoryInfo(FileManager.FolderPath);
        foreach (var dir in di.GetDirectories())
        {
            var sprite = iMG2Sprite.LoadNewSprite(FileManager.FolderPath + $"{currentSlot}Slot/Screenshot.png");
            SaveSlotData saveSlotData = new();
            saveSlotData.sprite = sprite;
            saveSlotData.id = Int32.Parse(dir.Name.Substring(0, 1));
            if (FileManager.LoadFromFile($"{dir.Name}/ClientSlot.sav", out var jsonC))
                saveSlotData.clientModel = JsonConvert.DeserializeObject<ClientModel>(jsonC);
            if (FileManager.LoadFromFile($"{dir.Name}/LevelSlot.sav", out var jsonL))
                saveSlotData.levelModel = JsonConvert.DeserializeObject<LevelModel>(jsonL);
            saveSlotDatas.Add(saveSlotData.id, saveSlotData);
        }
    }

    internal bool HasSlot(int id)
    {
        return saveSlotDatas.Any(slot => slot.Value.id == id);
    }

    internal bool HasSlot()
    {
        return saveSlotDatas.Any(slot => slot.Value.id == currentSlot);
    }

    internal SaveSlotData GetSlotData(int id)
    {
        return saveSlotDatas.First(slot => slot.Value.id == id).Value;
    }

    internal SaveSlotData GetSlotData()
    {
        SaveSlotData saveSlotData = null;
        foreach (var slotData in saveSlotDatas)
        {
            if (slotData.Value.id == currentSlot)
            {
                saveSlotData = slotData.Value;
                break;
            }
        }
        return saveSlotData;
    }

    public void MakeScrenshot()
    {
        ScreenCapture.CaptureScreenshot(FileManager.FolderPath + $"{currentSlot}Slot/Screenshot.png", 1);
    }

    private void SaveTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = FileManager.FolderPath + $"{currentSlot}Slot/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + "ScreenShot.png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
