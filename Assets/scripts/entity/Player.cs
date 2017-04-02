using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Unit[] units = new Unit[25];
    private Dictionary<UnitData, int> activeUnitsMap = new Dictionary<UnitData, int>();
    private Unit unitComponent;
    private int teamId;
    private int maxEnergy;
    private int energy;
    private int level;
    private int corruptedResource;
    private int createRange;

    public void InitPlayer() {
        // Placeholder stats
        // Initialize player
        this.energy = 10;
        this.createRange = 2;
        this.unitComponent = GetComponent<Unit>();
    }

    public Unit GetUnitComponent() {
        return this.unitComponent;
    }

    public int GetCreateRange() {
        return this.createRange;
    }

    public void SetTeamId(int teamId) {
        this.teamId = teamId;
    }

    public int GetTeamId() {
        return this.teamId;
    }

    public void ResetUnits() {
        foreach (Unit unit in this.units) {
            unit.SetHasMoved(false);
            unit.SetHasAttacked(false);
        }
    }

    public int GetEnergy() {
        return this.energy;
    }

    public void SetEnergy(int energy) {
        this.energy = energy;
    }

    public bool EnoughEnergy(int energy) {
        return this.energy >= energy;
    }

    public void SpendEnergy(int amount) {
        this.energy -= amount;
    }

    public int GetLevel() {
        return this.level;
    }

    public void LevelUp() {
        this.level++;
    }

    public int GetCorruptedResource() {
        return this.corruptedResource;
    }

    public void GainCorruptedResouce(int amount) {
        this.corruptedResource += amount;
    }

    public int GetUnitActiveCount(UnitData unit) {
        if (this.activeUnitsMap.ContainsKey(unit)) {
            return this.activeUnitsMap[unit];
        } else {
            this.activeUnitsMap[unit] = 0;
            return 0;
        }
    }
}
