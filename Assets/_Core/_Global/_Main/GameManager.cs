using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private int currentEnemyID;

    public int CurrentEnemyID => currentEnemyID;
    public ClientDeck ClientDeck => client.Deck;

    [Inject] private Client client;
    [Inject] private CardsList cardsList;


    public void SetCurrentEnemy(int val)
    {
        currentEnemyID = val;
    }

    private void Update()
    { }
}
