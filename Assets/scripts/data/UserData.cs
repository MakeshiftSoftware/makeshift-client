using System;

[Serializable]
public class UserData {
    public string username;
    public int level;
    public int activeBuild;
    public int unitLimit;
    public Build[] builds;

    public string GetUsername() {
        return this.username;
    }

    public int GetLevel() {
        return this.level;
    }

    public int GetUnitLimit() {
        return this.unitLimit;
    }

    public Build[] GetBuilds() {
        return this.builds;
    }

    public Build GetBuild(int index) {
        return this.builds[index];
    }

    public Build GetActiveBuild() {
        return this.builds[activeBuild];
    }
}
