using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private int currentEnemyID;

    public int CurrentEnemyID => currentEnemyID;

    [Inject] private Client client;
    [Inject] private CardsList cardsList;


    public void SetCurrentEnemy(int val)
    {
        currentEnemyID = val;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            client.Deck.AddCardToDeck(cardsList.list[Random.Range(0, cardsList.list.Count)]);
        }
    }
}
