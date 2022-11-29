using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Client
{
    private string client_name;

    private ClientDeck deck;
    private ClientModel model = new();

    public string Client_name => client_name;

    public ClientDeck Deck => deck;

    public void SetClient(string name, DefaultPlayerCards cardsList, int difficultIndex = 2)
    {
        client_name = name;
        model.hitPoint = difficultIndex switch
        {
            0 => -1,
            1 => 3,
            2 => 1
        };
        deck = new ClientDeck();
        foreach (var card in cardsList.list)
            Deck.AddCardToDeck(card);
    }

    public int FailBattle()
    {
        if (model.hitPoint > 0)
            model.hitPoint -= 1;
        return model.hitPoint;
    }

    public string GetJsonClientModel()
    {
        return GetClientModel().Get();
    }

    public ClientModel GetClientModel()
    {
        model.nameClient = client_name;
        model.date = DateTime.UtcNow;
        model.cardIDs.Clear();
        model.cardLevels.Clear();
        foreach (var card in Deck.Slots)
        {
            model.cardIDs.Add(card.card.id);
            model.cardLevels.Add(card.level);
        }
        return model;
    }

    public void SetClientModel(ClientModel data, CardsList cardsList)
    {
        model = data;

        client_name = model.nameClient;
        deck = new ClientDeck();
        foreach (var card in cardsList.list)
            if (model.Contains(card.id))
            {
                int level = model.GetCardLevel(card.id);
                for (var i = 0; i < level; i++)
                    Deck.AddCardToDeck(card);
            }
    }
}
