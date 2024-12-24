using InexperiencedDeveloper.Core;
using Riptide;
using Riptide.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ServerToClientMsg : ushort
{
    ApproveLogin,
}

public enum ClientToServerMsg : ushort
{
    RequestLogin,
}

public class NetworkManager : MonoBehaviour
{

    [SerializeField] private NetworkSettingsSO m_netSettings;
    private  void Awake()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    public Server Server { get; private set; }
    //[SerializeField] private ushort m_Port = 7777;
    //[SerializeField] private ushort m_MaxPlayers = 10;

    private void Start()
    {
        Server = new Server();
        Server.Start(m_netSettings.Port, m_netSettings.MaxPlayers);
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        NetworkEvents.SendMessageToPlayer += SendToPlayer;
        NetworkEvents.SendMessageToAll += SendToAll;

    }

    private void SendToAll(Message msg)
    {
        Server.SendToAll(msg);
    }

    private void SendToPlayer(Message msg, ushort id)
    {
        Server.Send(msg, id);
    }

    private void Unsubscribe()
    {
        NetworkEvents.SendMessageToPlayer -= SendToPlayer;
        NetworkEvents.SendMessageToAll -= SendToAll;
    }
    private void FixedUpdate()
    {
        Server.Update();
    }
}