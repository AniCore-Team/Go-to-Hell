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

    //public void SaveClient(string name)
    //{
    //    model.nameClient = client_name;
    //    model.cardIDs.Clear();
    //    model.cardLevels.Clear();
    //    foreach (var card in Deck.Slots)
    //    {
    //        model.cardIDs.Add(card.card.id);
    //        model.cardLevels.Add(card.level);
    //    }
    //    model.Save(name);
    //}

    //public bool LoadClient(string name, CardsList cardsList)
    //{
    //    bool hasLoad = model.Load(name);
    //    if (!hasLoad)
    //        return false;

    //    client_name = model.nameClient;
    //    deck = new ClientDeck();
    //    foreach (var card in cardsList.list)
    //        if (model.Contains(card.id))
    //        {
    //            int level = model.GetCardLevel(card.id);
    //            for (var i = 0; i < level; i++)
    //                Deck.AddCardToDeck(card);
    //        }
    //    return true;
    //}

    public string GetClientModel()
    {
        model.nameClient = client_name;
        model.cardIDs.Clear();
        model.cardLevels.Clear();
        foreach (var card in Deck.Slots)
        {
            model.cardIDs.Add(card.card.id);
            model.cardLevels.Add(card.level);
        }
        return model.Get();
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
