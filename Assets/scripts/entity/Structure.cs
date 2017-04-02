using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {
    private int damage;
    private int corruption;
    private bool isCorrupted;
    private bool isInvulnerable;
    private int armor;

    public bool IsCorrupted() {
        return this.isCorrupted;
    }

    public bool IsInvulnerable() {
        return this.isInvulnerable;
    }

    public int GetArmor() {
        return this.armor;
    }
}
