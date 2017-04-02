using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : Singleton<MainController> {
    private UserData user;
    private Dictionary<int, UnitData> units = new Dictionary<int, UnitData>();
    private Dictionary<int, GameObject> unitPrefabs = new Dictionary<int, GameObject>();

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);

        // temp
        OnLoginSuccess();
    }

    public UserData GetUser() {
        return this.user;
    }

    void OnLoginSuccess() {
        LoadGameData();
    }

    void OnLoginFailed() {
        
    }

    void LoadGameData() {
        TextAsset unitJson = Resources.Load("sampleunits") as TextAsset;
        TextAsset userJson = Resources.Load("sampleuser") as TextAsset;

        // Add unit data to unit dictionary
        foreach(UnitData data in JsonHelper.GetJsonArray<UnitData>(unitJson.text)) {
            this.units.Add(data.id, data);
        }

        // Load user data
        this.user = JsonUtility.FromJson<UserData>(userJson.text);

        // Load player builds data
        foreach(Build build in this.user.GetBuilds()) {
            foreach(UnitChoice choice in build.GetUnitChoices()) {
                UnitData data = this.units[choice.GetUnitId()];
                build.AddUnitData(data, choice.GetSlotId());
            }
        }

        Debug.Assert(this.user.GetUsername() == "taapesh");
        Debug.Assert(this.user.GetActiveBuild().GetName() == "Test Build 1");
        Debug.Assert(this.user.GetActiveBuild().GetUnit(1).name == "Awesome Unit");
    }

    public void TryLogin(string username, string password) {
        
    }

    public void Logout() {
        
    }

    public void StartGame() {
        // start matchmaking
        // load unit prefabs
        // load game scene
        // set gamemanager variables
        // initialize game
        // start game
    }
}
