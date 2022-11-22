using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zenject;

public class SaveManager
{
    [Inject] private Client client;
    [Inject] private CardsList cardsList;

    private LevelController currentLevel;
    private List<SaveSlotData> saveSlotDatas = new();
    private int currentSlot;

    public int CurrentSlot { set => currentSlot = value; }
    public LevelController CurrentLevel { set => currentLevel = value; }

    public void Save()
    {
        FileManager.WriteToFile($"{currentSlot}Slot/ClientSlot.sav", client.GetClientModel());
        FileManager.WriteToFile($"{currentSlot}Slot/LevelSlot.sav", currentLevel.GetLevelModel());
    }

    public void Load()
    {
        DirectoryInfo di = new DirectoryInfo(FileManager.FolderPath);
        foreach (var dir in di.GetDirectories())
        {
            SaveSlotData saveSlotData = new();
            saveSlotData.id = Int32.Parse(dir.Name.Substring(0, 1));
            if (FileManager.LoadFromFile($"{dir.Name}/ClientSlot.sav", out var jsonC))
                saveSlotData.clientModel = JsonConvert.DeserializeObject<ClientModel>(jsonC);
            if (FileManager.LoadFromFile($"{dir.Name}/LevelSlot.sav", out var jsonL))
                saveSlotData.levelModel = JsonConvert.DeserializeObject<LevelModel>(jsonL);
            saveSlotDatas.Add(saveSlotData);
        }
    }

    internal bool HasSlot(int id)
    {
        return saveSlotDatas.Any(slot => slot.id == id);
    }

    internal bool HasSlot()
    {
        return saveSlotDatas.Any(slot => slot.id == currentSlot);
    }

    internal SaveSlotData GetSlotData(int id)
    {
        return saveSlotDatas.First(slot => slot.id == id);
    }

    internal SaveSlotData GetSlotData()
    {
        return saveSlotDatas.First(slot => slot.id == currentSlot);
    }
}
