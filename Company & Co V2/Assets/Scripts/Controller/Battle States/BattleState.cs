using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State
{
    protected BattleController owner; //BattleController actuel

    //De la part de Turn
    public AbilityMenuPanelController abilityMenuPanelController
    {
        get { return owner.abilityMenuPanelController; }
    }
    public Turn turn { get { return owner.turn; } }
    public List<Unit> units { get { return owner.units; } }
    
    //Pour éviter de marquer owner.machin tout le temps
    public CameraRig cameraRig
    {
        get { return owner.cameraRig; }
    }
    public Board board
    {
        get { return owner.board; }
    }
    public LevelData levelData
    {
        get { return owner.levelData;  }
    }
    public Transform tileSelectionIndicator
    {
        get { return owner.tileSelectionIndicator; }
    }
    public Point pos
    {
        get { return owner.pos; }
        set { owner.pos = value; }
    }

    protected virtual void Awake() //Activé par Unity
    {
        owner = GetComponent<BattleController>();
    }

    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.fireEvent += OnFire;
    }

    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {

    }

    protected virtual void OnFire(object sender, InfoEventArgs<int> e)
    {

    }

    protected virtual void OnDestroy()
    {

    }


    protected virtual void SelectTile(Point p) //Met à jour le sélecteur de Tile (le machin jaune)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;

        pos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }
}
