using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public static int NUM_PLAYERS = 2;
	private Player[] players = new Player[NUM_PLAYERS];
    private int[] spawnTiles = new int[] { 28, 29 };
    private int turnId = 0;
    private int turnNumber = 1;
    private int secondsPerTurn = 10;
    private int turnTimer;
    private int lockCount;
    private HashSet<int> validTiles;
    private IEnumerator moveEnum;
    private bool gameStarted;
    private Build build;

    public delegate void OnTurnChangedEvent(int turnId);
    public static event OnTurnChangedEvent OnTurnChanged;

    public delegate void OnUnlockEvent();
    public static event OnUnlockEvent OnUnlock;

    public GameObject playerPrefab;

    void Start() {
        InitGame();
    }

    void InitGame() {
        this.build = MainController.Instance.GetUser().GetActiveBuild();
        int numTiles = TileManager.Instance.GenerateTiles();
        TileManager.Instance.InitTileArray(numTiles);
        SpawnPlayers();
        turnTimer = secondsPerTurn;
        InvokeRepeating("CountdownTurn", 0f, 1f);
    }

    public void Lock() {
        lockCount++;
    }

    public void Unlock() {
        lockCount--;

        if (lockCount == 0) {
            OnUnlock();
        }
    }

    public bool IsGameLocked() {
        return lockCount > 0;
    }

    private void CountdownTurn() {
        turnTimer -= 1;

        if (turnTimer == 0) {
            ChangeTurns();
        }
    }

    public void EndTurn() {
        return;
    }

    private void ChangeTurns() {
        turnNumber++;
        turnTimer = secondsPerTurn;
        turnId = 1 - turnId;
        OnTurnChanged(turnId);
    }

    public bool IsMyTurn(int teamId) {
        return turnId == teamId;
    }

    public bool CanMove(Unit unit) {
        return (
            unit.IsSelected() &&
            IsMyTurn(unit.GetTeamId()) &&
            !unit.HasMoved() &&
            !unit.HasAttacked() &&
            !IsGameLocked()
        );
    }

    public bool CanSummon(int teamId, int slotId) {
        Player player = GetPlayer(teamId);
        UnitData unit = this.build.GetUnit(slotId);

        if (!IsMyTurn(player.GetTeamId())) {
            Debug.Log("It is not my turn");
            return false;
        }

        if (!player.EnoughEnergy(unit.energyCost)) {
            Debug.Log("I don't have enough energy");
            return false;
        }

        if (player.GetUnitActiveCount(unit) == unit.activeLimit) {
            Debug.Log("I have too many of those right now");
            return false;
        }

        if (IsGameLocked()) {
            Debug.Log("I'm busy");
            return false;
        }

        return true;
    }

    public bool IsTileValid(int tileId) {
        return validTiles != null && validTiles.Contains(tileId);    
    }

    public void ToggleUnitMovement(Unit unit) {
        TileManager.Instance.DeactivateTiles();

        this.validTiles = TileManager.Instance
            .TilesInRange(unit.GetTileId(), unit.GetMovementRange());
        TileManager.Instance
            .ActivateTiles(this.validTiles, TileManager.Instance.moveMaterial);
    }

    public void ToggleCreateUnit(int teamId) {
        TileManager.Instance.DeactivateTiles();

        Player player = GetPlayer(teamId);
        Unit unit = player.GetUnitComponent();

        this.validTiles = TileManager.Instance
            .TilesInRange(unit.GetTileId(), player.GetCreateRange());
        TileManager.Instance
            .ActivateTiles(this.validTiles, TileManager.Instance.summonMaterial);
            
    }

    IEnumerator Move(Unit unit, Tile tile) {
        Vector3 target = tile.GetPosition();
        unit.SetDestination(target);

        while (true) {
            float magnitude = (unit.GetPosition() - target).sqrMagnitude;

            if (magnitude <= unit.GetStopDistance()) {
                StopCoroutine(this.moveEnum);
                this.Unlock();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void MoveUnitToTile(Unit unit, Tile tile) {
        this.Lock();
        tile.SetOccupant(unit);
        tile.ClearOccupant();
        unit.SetTileId(tile.GetTileId());
        unit.SetHasMoved(true);
        this.moveEnum = Move(unit, tile);
        StartCoroutine(this.moveEnum);
    }

    public Player GetPlayer(int teamId) {
        return players[teamId];
    }

    public Build GetBuild() {
        return this.build;
    }

    public void SpawnPlayers() {
        for (int i = 0; i < GameManager.NUM_PLAYERS; ++i) {
            Tile spawnTile = TileManager.Instance.GetTile(spawnTiles[i]);

            GameObject playerObj = (GameObject) Instantiate(
                playerPrefab,
                spawnTile.GetPosition(),
                Quaternion.identity);
            
            Unit unit = playerObj.GetComponent<Unit>();
            Player player = playerObj.GetComponent<Player>();
            player.SetTeamId(i);
            player.InitPlayer();
            unit.SetTeamId(i);
            unit.SetTileId(spawnTiles[i]);
            spawnTile.SetOccupant(unit);
			players[i] = player;
        }
    }
}
