using InexperiencedDeveloper.Core;
using Riptide;
using Riptide.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    protected override void Awake()
    {
        base.Awake();
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    public Server Server;
    [SerializeField] private ushort m_Port =7777;
    [SerializeField] private ushort m_Players = 4;
    private void Start()
    {
        Server = new Server();
        Server.Start(m_Port, m_Players);
    }

    private void Update()
    {
        Server.Update();
    }
}
