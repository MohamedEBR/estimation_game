using InexperiencedDeveloper.Core;
using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private NetworkSettingsSO m_netSettings;
    [SerializeField] private GameObject m_PlayerPrefab;
    private static GameObject s_PlayerPrefab;
    private static Dictionary<ushort, Player> s_Players = new Dictionary<ushort, Player>();
    public static Player GetPlayer(ushort id)
    {
        Debug.Log($"Getting player with ID: {id}");
        s_Players.TryGetValue(id, out Player player);

        if (player == null)
        {
            Debug.LogError($"Player with ID {id} not found in the dictionary!");
        }
        else
        {
            Debug.Log($"Player with ID {id} found: {player.name}");
        }

        return player;
    }
    public static bool RemovePlayer(ushort id)
    {
        if (s_Players.TryGetValue(id, out Player player))
        {
            s_Players.Remove(id);
            return true;
        }
        return false;
    }

    //public  Player LocalPlayer => GetPlayer(m_netSettings.LocalId);
    private static ushort s_LocalId = ushort.MaxValue;
    //public  bool IsLocalPlayer(ushort id) => id == LocalPlayer.Id;

    private  void Awake()
    {
        s_PlayerPrefab = m_PlayerPrefab;
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        NetworkEvents.ConnectSuccess += SpawnInitalPlayer;
    }

    private void Unsubscribe()
    {
        NetworkEvents.ConnectSuccess -= SpawnInitalPlayer;

    }

    public void SpawnInitalPlayer(ushort id, string username)
    {
        if (s_PlayerPrefab == null)
        {
            Debug.LogError("Static player prefab (s_PlayerPrefab) is null!");
            return;
        }

        GameObject playerObject = Instantiate(s_PlayerPrefab, Vector3.zero, Quaternion.identity);
        if (playerObject == null)
        {
            Debug.LogError("Failed to instantiate player prefab!");
            return;
        }

        Player player = playerObject.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component is missing on the instantiated prefab!");
            return;
        }

        if (id == 0)
        {
            Debug.LogError("NetworkManager's Client ID is 0 or invalid!");
            return;
        }

        player.name = $"{username} -- LOCAL PLAYER (WAITING FOR SERVER)";
        Debug.Log($"Spawning player with ID: {id}, Username: {username}");

        s_LocalId = id;
        player.Init(id, username, true);

        if (s_Players.ContainsKey(id))
        {
            Debug.LogError($"Player with ID {id} already exists in the dictionary!");
        }
        else
        {
            s_Players.Add(id, player);
            Debug.Log($"Player {username} with ID {id} successfully added to the dictionary.");
        }

        player.RequestInit();
    }

    private static void InitializeLocalPlayer()
    {

        Player local = s_Players[s_LocalId];
        local.name = $"{local.Username} -- {local.Id} -- LOCAL";
        Debug.Log($"Local player initialized: {local.name}");
    }

    #region Messages

    /* ================= MESSAGE Receiving ================*/
    [MessageHandler((ushort)ServerToClientMsg.ApproveLogin)]
    private static void ReceiveApproveLogin(Message msg)
    {
        bool approve = msg.GetBool();
        if (approve)
        {
            InitializeLocalPlayer();
        }
    }

    #endregion
}