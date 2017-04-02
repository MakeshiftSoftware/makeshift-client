using System;

[Serializable]
public class Friend
{
    public string username;
    public string _id;

    public string GetUsername()
    {
        return this.username;
    }

    public String GetId()
    {
        return this._id;
    }
}
