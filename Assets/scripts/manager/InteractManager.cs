using UnityEngine;

public class InteractManager : MonoBehaviour {
    private const int LEFT_MOUSE_BUTTON = 0;
    private const int RIGHT_MOUSE_BUTTON = 1;
    private GameManager gm;
    private LayerMask mask = -1;
    private UserData user;
    private Unit selectedUnit;
    public int teamId;

    public enum PlayerState {
        CreateReady,
        MoveReady,
        AbilityReady,
        Default
    };

    private PlayerState playerState = PlayerState.Default;

    void Awake() {
        this.gm = GameManager.Instance;
        GameManager.OnTurnChanged += this.OnTurnChanged;
        GameManager.OnUnlock += this.OnUnlock;
    }

    void Update () {
        if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON)) {
            Collider col = CheckForHit(Input.mousePosition);

            if (col != null) {
                switch (GetPlayerState()) {
                    case PlayerState.MoveReady:
                        AttemptMove(col);
                        break;
                    default:
                        break;
                }
            }
        }

        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
            Collider col = CheckForHit(Input.mousePosition);

            if (col != null) {
                switch (GetPlayerState()) {
                    case PlayerState.CreateReady:
                        AttemptCreate(col);
                        break;
                    case PlayerState.AbilityReady:
                        break;
                    default:
                        AttemptSelect(col);
                        break;
                }
            }
        }
    }

    void OnGUI() {
        if (this.gm.GetBuild().HasSlot(1) && GUI.Button(new Rect(10, Screen.height - 80, 60, 60),
            this.gm.GetBuild().GetUnit(1).name)) {

            if (this.gm.CanSummon(this.teamId, 1)) {
                Debug.Log("Create unit: " + this.gm.GetBuild().GetUnit(1).name);
                this.gm.ToggleCreateUnit(this.teamId);
                SetPlayerState(PlayerState.CreateReady);
            } else {
                Debug.Log("Can't create unit");
            }
        }

        if (this.gm.GetBuild().HasSlot(2) && GUI.Button(new Rect(100, Screen.height - 80, 60, 60),
            this.gm.GetBuild().GetUnit(2).name)) {
            
            if (this.gm.CanSummon(teamId, 1)) {
                Debug.Log("Create unit: " + this.gm.GetBuild().GetUnit(2).name);
                this.gm.ToggleCreateUnit(this.teamId);
                SetPlayerState(PlayerState.CreateReady);
            } else {
                Debug.Log("Can't create unit");
            }
        }
    }

    public void SetPlayerState(PlayerState playerState) {
        this.playerState = playerState;
    }

    private PlayerState GetPlayerState() {
        return this.playerState;
    }

    /*
     * Perform raycast and return collider if there was a hit
     * Otherwise, return null
     */
    private Collider CheckForHit(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value)) {
            return hit.collider;
        }
        return null;
    }

    private void AttemptMove(Collider col) {
        // Check if object is tile
        if (TileManager.Instance.IsTile(col)) {
            // Get tile object
            Tile tile = TileManager.Instance.GetTile(col);

            // Check if tile is valid
            if (this.gm.IsTileValid(tile.GetTileId())) {
                // Move unit to tile
                this.gm.MoveUnitToTile(selectedUnit, tile);
                TileManager.Instance.DeactivateTiles();
                SetPlayerState(PlayerState.Default);
            }
        } else {
            // Not tile
        }
    }

    private void AttemptCreate(Collider col) {
        if (TileManager.Instance.IsTile(col)) {
            Tile tile = TileManager.Instance.GetTile(col);

            if (this.gm.IsTileValid(tile.GetTileId())) {
                // Create unit on tile
                //GameManager.Instance.CreateUnit(teamId, 1);
                TileManager.Instance.DeactivateTiles();
            }
        }
    }


    private void AttemptSelect(Collider col) {
        Unit unit = col.GetComponent<Unit>();

        if (unit != null) {
            // Selected unit
            SelectUnit(unit);
        }
    }

    private void SelectUnit(Unit unit) {
        if (this.selectedUnit != null && unit.Equals(this.selectedUnit)) {
            // Selected same unit, do nothing
            return;
        }

        TileManager.Instance.DeactivateTiles();
        UnselectUnit();

        if (unit.IsFriendly(this.teamId)) {
            // Selected friendly unit
            unit.SetSelected(true);
            this.selectedUnit = unit;
            CheckForMovement();
        } else {
            // Selected hostile unit
            this.selectedUnit = unit;
        }
    }

    private void UnselectUnit() {
        if (this.selectedUnit != null) {
            this.selectedUnit.SetSelected(false);
            this.selectedUnit = null;
        }
    }

    private void CheckForMovement() {
        if (this.selectedUnit != null && this.selectedUnit.IsFriendly(this.teamId)) {
            if (this.gm.CanMove(selectedUnit)) {
                this.gm.ToggleUnitMovement(this.selectedUnit);
                SetPlayerState(PlayerState.MoveReady);
            }
        }
    }

    protected void OnUnlock() {
        if (this.gm.IsMyTurn(this.teamId)) {
            CheckForMovement();
        }
    }

    protected void OnTurnChanged(int turnId) {
        if (this.gm.IsMyTurn(this.teamId)) {
            CheckForMovement();
        } else {
            TileManager.Instance.DeactivateTiles();
        }
    }
}
