using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private int tileId;
    private Unit occupant;
    private Vector3 position;
    private Renderer _renderer;

    void Awake() {
        this.position = new Vector3(transform.position.x, 0, transform.position.z);
        this._renderer = GetComponent<Renderer>();
    }

    public int GetTileId() {
        return this.tileId;
    }

    public void SetTileId(int tileId) {
        this.tileId = tileId;
    }

    public bool IsAvailable() {
        return this.occupant == null; 
    }

    public Unit GetOccupant() {
        return this.occupant;
    }

    public void SetOccupant(Unit unit) {
        this.occupant = unit;
    }

    public void ClearOccupant() {
        this.occupant = null;
    }

    public Vector3 GetPosition() {
        return this.position;
    }

    public void SetMaterial(Material material) {
        this._renderer.material = material;
    }
}
