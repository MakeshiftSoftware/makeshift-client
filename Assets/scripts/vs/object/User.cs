using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class User
{
    public string username;
    public string token;
    public string _id;
    public int rank;
    public int level;
    public string gameId;
    public String activeBuildId;
    public Dictionary<String, Build> builds = new Dictionary<string, Build>();
    public Dictionary<String, Friend> friends = new Dictionary<string, Friend>();

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

    public Dictionary<String, Build> GetBuilds()
    {
        return this.builds;
    }

    public Build GetActiveBuild()
    {
        return this.builds[this.activeBuildId];
    }

    public Dictionary<String, Friend> GetFriends()
    {
        return this.friends;
    }

    public JSONArray GetFriendIds()
    {
        JSONArray ids = new JSONArray();

        foreach (String id in this.friends.Keys)
        {
            ids.Add(id);
        }

        return ids;
    }

    public void AddFriend(String id, Friend friend)
    {
        this.friends[id] = friend;
    }

    public void AddBuild(String id, Build build)
    {
        this.builds[id] = build;
    }
}
