using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Client
{
    private string client_name;

    private ClientDeck deck;

    public string Client_name => client_name;

    public ClientDeck Deck => deck;

    public void SetClient(string name, DefaultPlayerCards cardsList)
    {
        //Random.seed = (int)DateTime.UtcNow.Ticks;
        client_name = name;
        deck = new ClientDeck();
        foreach (var card in cardsList.list)
            Deck.AddCardToDeck(card);
    }
}
