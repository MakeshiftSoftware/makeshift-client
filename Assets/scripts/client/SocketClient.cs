using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public enum NetworkState
{
    CLOSED,
    CONNECTING,
    CONNECTED,
    DISCONNECTED,
    TIMEOUT,
    ERROR
}

public class SocketClient : IDisposable
{
    public static string HOST = "127.0.0.1";
    public static int SOCIAL_PORT = 8000;
    public static int MM_PORT = 8001;
    public static int GAME_PORT = 8002;

    // TODO: Add events to respond to changing network state
    public event Action<NetworkState> NetworkStateChangedEvent;

    private NetworkState networkState = NetworkState.CLOSED;

    private Socket socket;
    private Protocol protocol;
    private Action onConnected;
    private bool disposed;

    private ManualResetEvent timeoutEvent = new ManualResetEvent(false);
    private int timeoutMSec = 8000;

    public SocketClient()
    {
    }

    public void InitClient(string host, int port, Action callback = null)
    {
        timeoutEvent.Reset();
        NetworkChanged(NetworkState.CONNECTING);

        IPAddress ipAddress = null;

        try
        {
            IPAddress[] addresses = Dns.GetHostEntry(host).AddressList;

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = address;
                    break;
                }
            }
        }
        catch (Exception e)
        {
            NetworkChanged(NetworkState.ERROR);
            return;
        }

        if (ipAddress == null)
        {
            throw new Exception("Could not parse host: " + host);
        }

        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endpoint = new IPEndPoint(ipAddress, port);

        this.onConnected = callback;
        socket.BeginConnect(endpoint, OnConnected, this.socket);

        if (timeoutEvent.WaitOne(timeoutMSec, false))
        {
            if (networkState != NetworkState.CONNECTED && networkState != NetworkState.ERROR)
            {
                NetworkChanged(NetworkState.TIMEOUT);
                Dispose();
            }
        }
    }

    private void OnConnected(IAsyncResult result)
    {
        try
        {
            this.socket.EndConnect(result);
            this.protocol = new Protocol(this, this.socket);
            NetworkChanged(NetworkState.CONNECTED);

            if (this.onConnected != null)
            {
                this.onConnected();
            }
        }
        catch (SocketException e)
        {
            Debug.Log("Network error: " + e.Message);

            if (networkState != NetworkState.TIMEOUT)
            {
                NetworkChanged(NetworkState.ERROR);
            }

            Dispose();
        }
        finally
        {
            timeoutEvent.Set();
        }
    }

    private void NetworkChanged(NetworkState state)
    {
        networkState = state;

        if (this.NetworkStateChangedEvent != null)
        {
            this.NetworkStateChangedEvent(state);
        }
    }

    public void Disconnect()
    {
        Dispose();
        NetworkChanged(NetworkState.DISCONNECTED);
    }

    public void ConnectUserToSocket(User user)
    {
        this.protocol.Start();
        Message m = MessageBuilder.CreateConnectMessage(user.GetId());
        SendMessage(m);
    }

    public void StartMatchmaking(User user)
    {
        this.protocol.Start();
        Message m = MessageBuilder.CreateMatchmakingMessage(user);
        SendMessage(m);
    }

    public void JoinGame(User user)
    {
        this.protocol.Start();
        Message m = MessageBuilder.CreateJoinGameMessage(user);
        SendMessage(m);
    }

    public void SendMessage(Message m)
    {
        this.protocol.Send(m);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            if (this.protocol != null)
            {
                this.protocol.Close();
            }

            try
            {
                this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
                this.socket = null;
            }
            catch (Exception e)
            {

            }

            this.disposed = true;
        }
    }
}