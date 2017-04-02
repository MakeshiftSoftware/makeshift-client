using System;
using System.Net.Sockets;
using System.Text;

class StateObject
{
    public const int BufferSize = 1024;
    internal byte[] buffer = new byte[BufferSize];
    public StringBuilder sb = new StringBuilder();
}

public enum ProtocolState
{
    active = 1,
    closed = 2
}

public class Protocol
{
    private SocketClient client;
    private Socket socket;
    private StateObject stateObject;
    private ProtocolState protocolState;

    private bool onSending = false;
    private bool onReceiving = false;

    public Protocol(SocketClient client, Socket socket)
    {
        this.client = client;
        this.socket = socket;
        this.stateObject = new StateObject();
        this.protocolState = ProtocolState.active;
    }

    public void Start()
    {
        this.protocolState = ProtocolState.active;
        this.Receive();
    }

    public void Close()
    {
        this.protocolState = ProtocolState.closed;
    }

    public void Send(Message m)
    {
        if (protocolState != ProtocolState.closed)
        {
            byte[] bytes = GetMessageBytes(m);
            this.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(SendCallback), this.socket);
            this.onSending = true;
        }
    }

    private void Receive()
    {
        this.socket.BeginReceive(
            stateObject.buffer, 0, stateObject.buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), stateObject);
        this.onReceiving = true;
    }

    private void SendCallback(IAsyncResult result)
    {
        if (this.protocolState != ProtocolState.closed)
        {
            this.socket.EndSend(result);
            this.onSending = false;
        }
    }

    private void ReadCallback(IAsyncResult result)
    {
        if (this.protocolState == ProtocolState.closed)
        {
            return;
        }

        StateObject state = result.AsyncState as StateObject;

        try
        {
            int bytesRead = this.socket.EndReceive(result);
            this.onReceiving = false;

            if (bytesRead > 0)
            {
                ProcessBytes(state, bytesRead);

                if (this.protocolState != ProtocolState.closed)
                {
                    Receive();
                }
            }
        }
        catch (SocketException e)
        {
            // Todo
        }

    }

    private void ProcessBytes(StateObject state, int limit)
    {
        for (int i = 0; i < limit; ++i)
        {
            if (state.buffer[i] == 0) {
                OnMessageComplete();
            }
            else
            {
                char c = Convert.ToChar(state.buffer[i]);
                this.stateObject.sb.Append(c);
            }
        }
    }

    private byte[] GetMessageBytes(Message m)
    {
        string messageString = m.GetMessage() + Char.MinValue;
        byte[] bytes = ASCIIEncoding.UTF8.GetBytes(messageString);
        return bytes;
    }

    private void OnMessageComplete()
    {
        Message m = new Message(this.stateObject.sb.ToString());
        this.stateObject.sb.Length = 0;
        GameClient.messageHandler.OnMessage(m);
    }
}