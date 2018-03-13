using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    public GameObject heroPrefab;
    //public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }

    public AbilityMenuPanelController abilityMenuPanelController;
    public Turn turn = new Turn(); //Pour instancier les turns
    public List<Unit> units = new List<Unit>(); //Liste de toutes les unités dans la bataille

    // Use this for initialization
    void Start ()
    {
        //currentUnit = turn.actor;
        ChangeState<InitBattleState>();
	}
}
