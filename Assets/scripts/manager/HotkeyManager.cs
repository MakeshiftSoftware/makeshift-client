using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyManager : MonoBehaviour {
    private GameManager gm;
    public int teamId;

    void Awake() {
        this.gm = GameManager.Instance;
    }
}
