using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
[System.Serializable]
public class Match
{
    public string matchID;
    public List<Player> players = new();

    public Match(string matchID, Player player)
    {
        this.matchID = matchID;
        players.Add(player);
    }

    public Match() { }

}

//[System.Serializable]
//public class SyncListGameObject : SyncList<GameObject> { }

//[System.Serializable]
//public class SyncListMatch : SyncList<Match> { }

//public class SyncListString : SyncList<string> { }




public class MatchMaker : NetworkBehaviour
{
    public static MatchMaker instance;

    public SyncList<Match> matches = new();

    public SyncList<string> matchIDs = new();


    [SerializeField] GameObject turnManagerPrefab;
    void Start()
    {
        instance = this;
    }
    public bool HostGame(string _matchID, Player _player, out int playerIndex)
    {
        playerIndex = -1;
        if (!matchIDs.Contains(_matchID))
        {
            matchIDs.Add(_matchID);
            matches.Add(new Match(_matchID, _player));
            Debug.Log("Match Id generated");
            playerIndex = 1;
            return true;
        }
        else
        {
            Debug.Log("Match Id already exists");
            return false;
        }

    }

    public void BeginGame(string _matchID)
    {
        GameObject newTurnManager = Instantiate(turnManagerPrefab);
        newTurnManager.GetComponent<NetworkMatch>().matchId = _matchID.ToGuid();
        TurnManager turnManager = newTurnManager.GetComponent<TurnManager>();

        for (int i = 0; i < matches.Count; i++) {   
            if(matches[i].matchID == _matchID){
                foreach (var player in matches[i].players) { 
                    Player _player = player.GetComponent<Player>();
                    turnManager.AddPlayer(_player);
                    _player.StartGame();
                }
                break;
            }
        }
    }

    public bool JoinGame(string _matchID, Player _player, out int playerIndex)
    {
        playerIndex = -1;
        if (matchIDs.Contains(_matchID))
        {

            for (int i = 0; i< matches.Count ; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    matches[i].players.Add(_player);
                    playerIndex = matches[i].players.Count;
                    break;
                }
            }
            Debug.Log("Match Joined");
            return true;
        }
        else
        {
            Debug.Log("Match Id does not exist");
            return false;
        }

    }
    public static string GetRandomMatchID()
    {
        string _id = string.Empty;
        for (var i = 0; i < 5; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26)
            {
                _id += (char)(random + 65);
            }
            else
            {
                _id += (random - 26).ToString();
            }
        }
        Debug.Log($"Random Match ID = {_id}");
        return _id;
    }

    
}

public static class MatchExtenstions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);
        return new Guid(hashBytes);
    }
}
