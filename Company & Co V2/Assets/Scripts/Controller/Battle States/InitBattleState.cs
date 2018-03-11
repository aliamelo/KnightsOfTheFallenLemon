using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits(); //Venez mes unitééééés
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }

    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[] { typeof(WalkMovement), typeof(FlyMovement), typeof(TeleportMovement) };
        for(int i = 0; i < 3; ++i)
        {
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();

            Movement m = instance.AddComponent(components[i]) as Movement;
            m.range = 5;
            m.jumpHeight = 1;

            units.Add(unit);
        }
    }
}
