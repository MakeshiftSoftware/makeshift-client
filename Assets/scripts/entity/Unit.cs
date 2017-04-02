using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent nav;
    private Transform _transform;
    private int teamId;
    private int unitId;
    private int tileId;
    private bool hasMoved;
    private bool hasAttacked;
    private bool isDead;
    private bool isSelected;
    public float stopDistance;
    public string unitName;
    public int health;
    public int maxHealth;
    public int movementRange;
    public int activeLimit;
    public int energyCost;

    void Awake() {
        this._transform = transform;
        this.nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public bool IsFriendly(int teamId) {
        return this.teamId == teamId;
    }

    public void SetDestination(Vector3 target) {
        nav.SetDestination(target);
    }

    public float GetStopDistance() {
        return this.stopDistance;
    }

    public void SetSelected(bool selected) {
        this.isSelected = selected;
    }

    public bool IsSelected() {
        return this.isSelected;
    }

    public int GetUnitId() {
        return this.unitId;
    }

    public void SetUnitId(int unitId) {
        this.unitId = unitId;
    }

    public int GetTeamId() {
        return this.teamId;
    }

    public void SetTeamId(int teamId) {
        this.teamId = teamId;
    }

    public int GetTileId() {
        return this.tileId;
    }

    public void SetTileId(int tileId) {
        this.tileId = tileId;
    }

    public bool HasMoved() {
        return this.hasMoved;
    }

    public void SetHasMoved(bool moved) {
        this.hasMoved = moved;
    }

    public bool HasAttacked() {
        return this.hasAttacked;
    }

    public void SetHasAttacked(bool hasAttacked) {
        this.hasAttacked = false;
    }

    public bool IsDead() {
        return this.isDead;
    }

    public int GetMovementRange() {
        return this.movementRange;
    }

    public int GetActiveLimit() {
        return this.activeLimit;
    }

    public int GetEnergyCost() {
        return this.energyCost;
    }

    public bool Equals(Unit unit) {
        return (
            unit.GetTeamId() == this.teamId &&
            unit.GetUnitId() == this.unitId
        );
    }

    public Vector3 GetPosition() {
        return new Vector3(this._transform.position.x, 0, this._transform.position.z);
    }

    public void SetUnitData(UnitData data) {
        this.unitName = data.name;
        this.maxHealth = data.maxHealth;
        this.health = data.maxHealth;
        this.movementRange = data.movementRange;
        this.activeLimit = data.activeLimit;
        this.energyCost = data.energyCost;
    }
}
