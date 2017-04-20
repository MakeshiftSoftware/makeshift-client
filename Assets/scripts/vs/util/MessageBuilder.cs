using System;
using SimpleJSON;

class MessageBuilder
{
    public static Message CreateConnectMessage(string userId)
    {
        Message m = new Message(MessageType.CONNECT);
        m.Add(MessageProperty.PLAYER, userId);
        return m;
    }

    public static Message CreateMatchmakingMessage(User user)
    {
        // TODO: add options to matchmaking
        // Temporary: Every player is rank 1
        int rank = user.GetRank();
        if (rank == 0) rank = 1;

        Message m = new Message(MessageType.QUEUE);
        m.Add(MessageProperty.PLAYER, user.GetId());
        m.Add(MessageProperty.RANK, rank);
        return m;
    }

    public static Message CreateJoinGameMessage(User user)
    {
        Message m = new Message(MessageType.JOIN);
        m.Add(MessageProperty.PLAYER, user.GetId());
        m.Add(MessageProperty.GAME, user.GetGameId());
        m.Add(MessageProperty.USERNAME, user.GetUsername());
        return m;
    }

    public static Message CreateNotifyOnlineMessage(User user)
    {
        Message m = new Message(MessageType.NOTIFY_ONLINE);
        m.Add(MessageProperty.RECIPIENTS, user.GetFriendIds());
        return m;
    }
}