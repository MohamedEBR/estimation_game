using System.Collections.Generic;
using Mirror;
using NUnit.Framework;
using UnityEngine;

public class TurnManager : NetworkBehaviour
{

    List<Player> players = new();
    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}
