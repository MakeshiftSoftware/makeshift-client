using System;
using SimpleJSON;

public class Message
{
    JSONNode msg;

    public Message(string json)
    {
        this.msg = JSON.Parse(json);
    }

    public Message(int type)
    {
        this.msg = new JSONObject();
        this.msg.Add(MessageProperty.TYPE, type);
    }

    public string GetMessage()
    {
        return this.msg.ToString();
    }

    public void Add(string key, JSONNode value)
    {
        this.msg.Add(key, value);
    }

    public JSONNode Get(string key)
    {
        return this.msg[key];
    }
}

