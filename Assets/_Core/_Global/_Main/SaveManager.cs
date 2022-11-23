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
    [Inject] private CardsList cardsList;

    //private LevelController currentLevel;
    private IMG2Sprite iMG2Sprite = new();
    private Dictionary<int, SaveSlotData> saveSlotDatas = new();
    private int currentSlot;

    public int CurrentSlot { set => currentSlot = value; }
    //public LevelController CurrentLevel { set => currentLevel = value; }

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
        return saveSlotData;// s.First(slot => slot.Value.id == currentSlot).Value;
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

public class IMG2Sprite// : MonoBehaviour
{

    // This script loads a PNG or JPEG image from disk and returns it as a Sprite
    // Drop it on any GameObject/Camera in your scene (singleton implementation)
    //
    // Usage from any other script:
    // MySprite = IMG2Sprite.instance.LoadNewSprite(FilePath, [PixelsPerUnit (optional)])

    //private static IMG2Sprite _instance;

    //public static IMG2Sprite instance
    //{
    //    get
    //    {
    //        //If _instance hasn't been set yet, we grab it from the scene!
    //        //This will only happen the first time this reference is used.

    //        if (_instance == null)
    //            _instance = GameObject.FindObjectOfType<IMG2Sprite>();
    //        return _instance;
    //    }
    //}

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
