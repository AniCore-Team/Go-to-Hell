using UnityEditor.PackageManager;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private int currentEnemyID;
    private int indexEvent = -1;
    private bool isUsedEvent;

    public int CurrentEnemyID => currentEnemyID;
    public ClientDeck ClientDeck => client.Deck;
    public CardsList CardsList => cardsList;

    [Inject] private Client client;
    [Inject] private CardsList cardsList;
    [Inject] private SaveManager saveManager;
    [Inject] private LoadingManager loadingManager;

    public bool IsBossBattle { get; set; }

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

    public bool IsFailGame()
    {
        int hitPoint = client.FailBattle();

        if (hitPoint == 0)
        {
            saveManager.ClearSlot();
            loadingManager.LoadScene("Menu");
            return true;
        }
        return false;
    }
}
