using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MessageHandler
{
    private GameClient client;
    private Dictionary<int, Action<Message>> eventMap;

    public MessageHandler(GameClient client)
    {
        this.client = client;
		this.eventMap = new Dictionary<int, Action<Message>>();
        this.eventMap.Add(MessageType.CONNECTED, OnUserOnline);
        this.eventMap.Add(MessageType.CHAT, OnChatMessage);
        this.eventMap.Add(MessageType.MATCH, OnMatchFound);
        this.eventMap.Add(MessageType.NO_MATCH, OnMatchNotFound);
    }

    public void OnMessage(Message m)
    {
        int messageType = m.Get(MessageProperty.TYPE);

        if (this.eventMap.ContainsKey(messageType))
        {
            this.eventMap[messageType](m);
        }
    }

    private void OnChatMessage(Message m)
    {
        Debug.Log("Chat received: " + m.GetMessage());
    }

    private void OnUserOnline(Message m)
    {
        Debug.Log("User online");
        this.client.OnUserOnline();
    }

    private void OnMatchFound(Message m)
    {
        Debug.Log("Match found, loading game...");
        this.client.JoinGame(m.Get(MessageProperty.GAME));
    }

    private void OnMatchNotFound(Message m)
    {
        Debug.Log("No match found, retrying...");
    }
}