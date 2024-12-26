using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UILobby : MonoBehaviour
{
    public static UILobby instance;


    [SerializeField] TMP_InputField joinMatchInput;
    [SerializeField] Button joinButton;
    [SerializeField] Button hostButton;
    [SerializeField] Canvas lobbyCanvas;


    void Start()
    {
        instance = this;   
    }
    public void Host()
    {
        joinMatchInput.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;

        if (Player.localPlayer == null)
        {
            Debug.LogError("localPlayer is null! Ensure Player.localPlayer is properly assigned.");
            return;
        }

        Player.localPlayer.HostGame();
    }

    public void HostSuccess(bool success)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
        }
        else
        {

            joinMatchInput.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;

        }
    }
    public void Join()
    {
        joinMatchInput.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;

        Player.localPlayer.JoinGame(joinMatchInput.text);  
    }

    public void JoinSuccess(bool success)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
        } else
        {
            joinMatchInput.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
        }
    }
}
