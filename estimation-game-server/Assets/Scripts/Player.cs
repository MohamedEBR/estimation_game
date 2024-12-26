using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class Player : NetworkBehaviour
{

    public static Player localPlayer;
    
    [SyncVar] public string matchID;
    [SyncVar] public int playerIndex;

    NetworkMatch networkMatch;

    void Start()
    {
        networkMatch = GetComponent<NetworkMatch>();

        if (isLocalPlayer)
        {
            localPlayer = this;
            Debug.Log("localPlayer assigned.");
        } else
        {
            UILobby.instance.SpawnPlayerPrefab(this);
        }


    }

    /* 
    Host Match
    */
    public void HostGame()
    {
        string matchID = MatchMaker.GetRandomMatchID();
        CmdHostGame(matchID);
    }
    [Command]
    void CmdHostGame(string _matchID)
    {
        matchID = _matchID;
       if(MatchMaker.instance.HostGame(_matchID, this, out int playerIndex))
        {

            Debug.Log($"<color = green>Game Hosted Successfully</color>");
            networkMatch.matchId = _matchID.ToGuid();
            TargetHostGame(true, _matchID);
        } else
        {
            Debug.Log($"<color = red>Game Hosted Failed</color>");
            TargetHostGame(false, _matchID);
        }
    }

    [TargetRpc]
    void TargetHostGame(bool success, string _matchID)
    {
        Debug.Log($"MatchID: {matchID} == {_matchID}");
        UILobby.instance.HostSuccess(success);
    }

    /* 
     Join Match
     */
    public void JoinGame(string _inputID)
    {
        CmdJoinGame(_inputID);
    }

    [Command]
    void CmdJoinGame(string _matchID)
    {
        matchID = _matchID;
        if (MatchMaker.instance.JoinGame(_matchID, this, out int playerIndex))
        {

            Debug.Log($"<color = green>Game Joined Successfully</color>");
            networkMatch.matchId = _matchID.ToGuid();
            TargetJoinGame(true, _matchID);
        }
        else
        {
            Debug.Log($"<color = red>Game Joined Failed</color>");
            TargetJoinGame(false, _matchID);
        }
    }

    [TargetRpc]
    void TargetJoinGame(bool success, string _matchID)
    {
        Debug.Log($"MatchID: {matchID} == {_matchID}");
        UILobby.instance.JoinSuccess(success);
    }

    /* 
    Begin Match
    */
    public void BeginGame()
    {
        CmdBeginGame();
    }

    [Command]
    void CmdBeginGame()
    {
        MatchMaker.instance.BeginGame(matchID);
        Debug.Log("Game Beginning");
    }


    public void StartGame()
    {
        TargetBeginGame();

    }
    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"MatchID: {matchID} | Beginning");

        //Additively load game scene
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

}
