using Riptide;
using UnityEngine.Events;

public static class NetworkEvents
{



    public static event UnityAction<Message> SendMessageToAll;
    public static void Send(Message message) => SendMessageToAll?.Invoke(message);
    public static event UnityAction<Message, ushort> SendMessageToPlayer;
    public static void Send(Message msg, ushort id) => SendMessageToPlayer?.Invoke(msg, id);
}
