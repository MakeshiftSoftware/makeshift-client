using System;
using System.Collections.Generic;

[Serializable]
public class Build
{
    private Dictionary<int, UnitData> units = new Dictionary<int, UnitData>();
    public string name;
    public bool isActive;

    public string GetName()
    {
        return this.name;
    }

    public bool IsActive()
    {
        return this.isActive;
    }

    public bool HasSlot(int slot)
    {
        return this.units[slot] != null;
    }

    public UnitData GetUnit(int slot)
    {
        return this.units[slot];
    }

    public Dictionary<int, UnitData> GetUnits()
    {
        return this.units;
    }

    public void AddUnit(int slot, UnitData data)
    {
        this.units.Add(slot, data);
    }
}