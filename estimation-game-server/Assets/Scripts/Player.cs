using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
{

    public static Player localPlayer;
    
    [SyncVar] public string matchID;

    NetworkMatch networkMatch;

    void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
            Debug.Log("localPlayer assigned.");
        }

        networkMatch = GetComponent<NetworkMatch>();

    }
    public void HostGame()
    {
        string matchID = MatchMaker.GetRandomMatchID();
        CmdHostGame(matchID);
    }
    [Command]
    void CmdHostGame(string _matchID)
    {
        matchID = _matchID;
       if(MatchMaker.instance.HostGame(_matchID, this))
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

    public void JoinGame(string _inputID)
    {
        CmdJoinGame(_inputID);
    }

    [Command]
    void CmdJoinGame(string _matchID)
    {
        matchID = _matchID;
        if (MatchMaker.instance.JoinGame(_matchID, this))
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
}
