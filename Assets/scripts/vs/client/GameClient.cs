 using System;
using SimpleJSON;
using UnityEngine;

public class GameClient
{
    private User user;
    private SocketClient socialClient;
    private SocketClient matchmakingClient;
    private SocketClient gameClient;
    private Action loginSuccess;
    private Action loginError;

    public static MessageHandler messageHandler;
    public GameController controller;

    public GameClient(GameController controller)
    {
        this.controller = controller;
        this.socialClient = new SocketClient();
        this.matchmakingClient = new SocketClient();
        this.gameClient = new SocketClient();
        messageHandler = new MessageHandler(this);
    }

    public User GetUser()
    {
        return this.user;
    }

    public void Login(string username, string password, Action success = null, Action error = null)
    {
        this.loginSuccess = success;
        this.loginError = error;
        HttpRequest request = new HttpRequest();
        JSONObject data = new JSONObject();
        data.Add("username", username);
        data.Add("password", password);
        request.PostAsync("http://127.0.0.1:3000/api/v1/auth/login", data, OnLoginResponse);
    }

    public void Logout(Action callback = null)
    {
        
    }
    
    private void OnLoginResponse(HttpResponse response)
    {
        if (response.Success())
        {
            this.user = JsonUtility.FromJson<User>(response.Body());
            this.socialClient.InitClient(SocketClient.HOST, SocketClient.SOCIAL_PORT, OnConnectedToSocialServer);
        }
        else
        {
            if (this.loginError != null)
            {
                this.loginError();
            }
        }
    }

    public void OnConnectedToSocialServer()
    {
        Debug.Log("Connected to social server");
        this.socialClient.ConnectUserToSocket(this.user);
    }

    public void OnUserOnline()
    {
        if (this.loginSuccess != null)
        {
            this.loginSuccess();
        }

        Message m = MessageBuilder.CreateNotifyOnlineMessage(this.user);
        this.socialClient.SendMessage(m);

        // Temporary: Connect to matchmaking and try to find match immediately
        ConnectToMatchmakingServer();
    }

    public void ConnectToMatchmakingServer()
    {
        // TODO: include options for matchmaking
        this.matchmakingClient.InitClient(SocketClient.HOST, SocketClient.MM_PORT, FindGame);
    }

    private void FindGame()
    {
        Debug.Log("Connected to matchmaking server. Finding game");
        this.matchmakingClient.StartMatchmaking(this.user);
    }

    public void JoinGame(string gameId)
    {
        Debug.Log("Joining game with id: " + gameId);
        this.user.SetGameId(gameId);
        // TODO: Disconnect from matchmaking server, connect to game server
        // TODO: Post request to set user's active game id
        ConnectToGameServer();
    }

    private void ConnectToGameServer()
    {
        Debug.Log("Connecting to game server...");
        this.gameClient.InitClient(SocketClient.HOST, SocketClient.GAME_PORT, OnConnectedToGameServer);
    }

    private void OnConnectedToGameServer()
    {
        Debug.Log("Connected to game server");
        this.gameClient.JoinGame(this.user);
    }

    public void SendChat()
    {

    }

    public void Disconnect()
    {
        // TODO: Other cleanup tasks
        this.socialClient.Disconnect();
    }
}

