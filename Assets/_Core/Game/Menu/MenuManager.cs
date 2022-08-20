using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button newGameButton;

    private Client client;

    private CreateNewGameWindow createNewGame;

    [Inject]
    public void Construct(Client client, CreateNewGameWindow createNewGame)
    {
        this.client = client;
        this.createNewGame = createNewGame;
    }

    private void Awake()
    {
        EventsTranslator.AddListener(WindowsTag.Create, createNewGame.SetProperty);
    }

    void Start()
    {
        Utils.AddListenerToButton(newGameButton, CreateNewGame);
    }

    public void CreateNewGame()
    {
        EventsTranslator.SendListener(WindowsTag.Create);
    }

    private void OnDestroy()
    {
        EventsTranslator.RemoveAllListeners();
    }
}

public static class WindowsTag
{
    public const string Create = "Create";
}