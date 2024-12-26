using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UILobby : MonoBehaviour
{
    public static UILobby instance;

    [Header("Host Join")] 
    [SerializeField] TMP_InputField joinMatchInput;
    [SerializeField] Button joinButton;
    [SerializeField] Button hostButton;
    [SerializeField] Canvas lobbyCanvas;


    [Header("Lobby")]
    [SerializeField] Transform UIPlayerParent;
    [SerializeField] GameObject UIPlayerPrefab;
    [SerializeField] TMP_Text matchIDText;
    [SerializeField] GameObject beginGameButron;

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
            SpawnPlayerPrefab(Player.localPlayer);
            matchIDText.text = Player.localPlayer.matchID;
            beginGameButron.SetActive(true);
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
        Player.localPlayer.JoinGame(joinMatchInput.text.ToUpper());  
    }

    public void JoinSuccess(bool success)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            SpawnPlayerPrefab(Player.localPlayer);

            matchIDText.text = Player.localPlayer.matchID;

        }
        else
        {
            joinMatchInput.interactable = true;
            joinButton.interactable = true;
            hostButton.interactable = true;
        }
    }

    public void SpawnPlayerPrefab(Player player)
    {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
        newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
    }

    public void BeginGame()
    {
        Player.localPlayer.BeginGame();
    }
}
