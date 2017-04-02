using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class UnitData {
    public int id;
    public string name;
    public int movementRange;
    public int energyCost;
    public int maxHealth;
    public int activeLimit;
    public int createCooldown;

    public int GetId() {
        return this.id;
    }

    public string GetName() {
        return this.name;
    }

    public int GetMovementRange() {
        return this.movementRange;
    }

    public int GetEnergyCost() {
        return this.energyCost;
    }

    public int GetMaxHealth() {
        return this.maxHealth;
    }

    public int GetActiveLimit() {
        return this.activeLimit;
    }

    public int GetCreateCooldown() {
        return this.createCooldown;
    }
}
