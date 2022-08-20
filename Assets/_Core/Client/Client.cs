using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client
{
    private string client_name;

    private ClientDeck deck;

    public string Client_name => client_name;

    public ClientDeck Deck => deck;

    public void SetClient(string name)
    {
        client_name = name;
        deck = new ClientDeck();
    }
}
