using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class Client
{
    private string client_name;

    private ClientDeck deck;
    private ClientModel model = new();

    public string Client_name => client_name;

    public ClientDeck Deck => deck;

    public void SetClient(string name, DefaultPlayerCards cardsList)
    {
        client_name = name;
        deck = new ClientDeck();
        foreach (var card in cardsList.list)
            Deck.AddCardToDeck(card);
    }

    public void SaveClient(string name)
    {
        model.nameClient = client_name;
        model.cardIDs.Clear();
        model.cardLevels.Clear();
        foreach (var card in Deck.Slots)
        {
            model.cardIDs.Add(card.card.id);
            model.cardLevels.Add(card.level);
        }
        model.Save(name);
    }

    public bool LoadClient(string name, CardsList cardsList)
    {
        bool hasLoad = model.Load(name);
        if (!hasLoad)
            return false;

        client_name = model.nameClient;
        deck = new ClientDeck();
        foreach (var card in cardsList.list)
            if (model.Contains(card.id))
            {
                int level = model.GetCardLevel(card.id);
                for (var i = 0; i < level; i++)
                    Deck.AddCardToDeck(card);
            }
        return true;
    }
}

[Serializable]
public class ClientModel
{
    public string nameClient = "";
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
}
