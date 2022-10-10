using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class CreateNewGameWindow : UIWindow
{
    [SerializeField] private TMP_InputField field;
    [SerializeField] private Button play, cancel;
    [SerializeField] private GameObject errorText;

    [Inject] private Client client;
    [Inject] private CardsList cardsList;

    public void SetProperty()
    {
        Utils.AddListenerToButton(play, Play);
        Utils.AddListenerToButton(cancel, Cancel);
        ActivateWindow();
        field.text = $"Player{Random.Range(1, 10000)}";
    }

    public void Play()
    {
        if(field.text == "")
        {
            errorText.SetActive(true);
        }
        else
        {
            client.SetClient(field.text, cardsList);
            errorText.SetActive(false);
            LoadingManager.OnLoadScene.Invoke("Location");
            DisactivateWindow();
        }
    }

    public void Cancel()
    {
        errorText.SetActive(false);
        DisactivateWindow();
    }
}