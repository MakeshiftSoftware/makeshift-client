using System;

[Serializable]
public class User
{
    public string username;
    public string token;
    public string _id;
    public int rank;
    public Friend[] friends;
    public string gameId;

    public string GetToken()
    {
        return this.token;
    }

    public string GetUsername()
    {
        return this.username;
    }

    public string GetId()
    {
        return this._id;
    }

    public int GetRank()
    {
        return this.rank;
    }

    public void SetGameId(string gameId)
    {
        this.gameId = gameId;
    }

    public string GetGameId()
    {
        return this.gameId;
    }
}
