using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ClientModel
{
    public string nameClient = "";
    public int hitPoint = -1;
    public DateTime date;
    public List<CardID> cardIDs = new();
    public List<int> cardLevels = new();

    public bool Contains(CardID id)
    {
        return cardIDs.Any(c => c == id);
    }

    public int GetCardLevel(CardID id)
    {
        return cardLevels[cardIDs.IndexOf(id)];
    }

    public void Save(string name)
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        FileManager.WriteToFile($"{name}.sav", json);
    }

    public bool Load(string name)
    {
        if (FileManager.LoadFromFile($"{name}.sav", out var json))
        {
            ClientModel loadData = JsonConvert.DeserializeObject<ClientModel>(json);
            nameClient = loadData.nameClient;
            cardIDs = loadData.cardIDs;
            cardLevels = loadData.cardLevels;
            return true;
        }
        return false;
    }

    public string Get()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public void Set(string data)
    {
        ClientModel loadData = JsonConvert.DeserializeObject<ClientModel>(data);
        nameClient = loadData.nameClient;
        cardIDs = loadData.cardIDs;
        cardLevels = loadData.cardLevels;
    }
}
