using System;

[Serializable]
public class UnitChoice {
    public int unitId;
    public int slotId;

    public int GetUnitId() {
        return unitId;
    }

    public int GetSlotId() {
        return slotId;
    }
}
