using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private int currentEnemyID;
    private int indexEvent = -1;
    private bool isUsedEvent;

    public int CurrentEnemyID => currentEnemyID;
    public ClientDeck ClientDeck => client.Deck;

    [Inject] private Client client;
    [Inject] private CardsList cardsList;


    public void SetCurrentEnemy(int val)
    {
        currentEnemyID = val;
    }

    public void SetUsingEvent(int indexEvent)
    {
        this.indexEvent = indexEvent;
    }

    public void SetUsedEvent()
    {
        isUsedEvent = true;
    }

    public int GetUsedIndexEvent()
    {
        if (isUsedEvent)
        {
            isUsedEvent = false;
            return indexEvent;
        }
        return -1;
    }
}
