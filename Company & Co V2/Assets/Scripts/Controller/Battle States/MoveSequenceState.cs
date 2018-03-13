using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        Movement m = turn.actor.GetComponent<Movement>();
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        turn.hasUnitMoved = true;
        owner.ChangeState<CommandSelectionState>();
    }
}
