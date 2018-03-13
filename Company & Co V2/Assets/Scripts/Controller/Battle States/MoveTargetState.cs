using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    List<Tile> tiles;

    public override void Enter()
    {
        base.Enter();
        Movement mover = turn.actor.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(board);
        board.SelectTiles(tiles);
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles = null;
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
                owner.ChangeState<MoveSequenceState>();
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
}
