using System.Collections.Generic;
using System;

[Serializable]
public class Build {
    private Dictionary<int, UnitData> units = new Dictionary<int, UnitData>();
    public UnitChoice[] unitChoices;
    public string name;

    public string GetName() {
        return this.name;
    }

    public Dictionary<int, UnitData> GetUnits() {
        return this.units;
    }

    public UnitChoice[] GetUnitChoices() {
        return this.unitChoices;
    }

    public void AddUnitData(UnitData unitData, int slotId) {
        this.units.Add(slotId, unitData);
    }

    public UnitData GetUnit(int slotId) {
        return this.units[slotId];
    }

    public bool HasSlot(int slotId) {
        return this.units[slotId] != null;
    }
}
