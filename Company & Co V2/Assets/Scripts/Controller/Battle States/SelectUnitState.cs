using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState
{
    int index = -1;

    public override void Enter()
    {
        base.Enter();
        StartCoroutine("ChangeCurrentUnit");
    }

    IEnumerator ChangeCurrentUnit()
    {
        index = (index + 1) % units.Count;
        turn.Change(units[index]);
        yield return null;
        owner.ChangeState<CommandSelectionState>();
    }
}
