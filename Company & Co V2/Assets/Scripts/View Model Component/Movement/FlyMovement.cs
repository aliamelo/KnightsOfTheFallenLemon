using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : Movement
{
    public override IEnumerator Traverse(Tile tile)
    {
        //Sauvegarde la distance entre la Tile de départ et celle d'arrivée
        float dist = Mathf.Sqrt(Mathf.Pow(tile.pos.x - unit.tile.pos.x, 2) + Mathf.Pow(tile.pos.y - unit.tile.pos.y, 2));
        unit.Place(tile);
        
        //Vole assez haut pour pas se prendre des Tiles
        float y = Tile.stepHeight * 10;
        float duration = (y - jumper.position.y) * 0.5f;

        Tweener tweener = jumper.MoveToLocal(new Vector3(0, y, 0), duration, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
        
        //Tourne dans la direction générale
        Direction dir;
        Vector3 toTile = (tile.center - transform.position);
        if (Mathf.Abs(toTile.x) > Mathf.Abs(toTile.z))
            dir = toTile.x > 0 ? Direction.East : Direction.West;
        else
            dir = toTile.z > 0 ? Direction.North : Direction.South;
        yield return StartCoroutine(Turn(dir));
        
        //Bouge dans la position correcte
        duration = dist * 0.5f;
        tweener = transform.MoveTo(tile.center, duration, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
        
        //Atteris
        duration = (y - tile.center.y) * 0.5f;
        tweener = jumper.MoveToLocal(Vector3.zero, 0.5f, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;
    }
}
