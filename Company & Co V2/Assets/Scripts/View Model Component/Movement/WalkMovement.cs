using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement
{
    //Ajouter le code ici
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        //Saute si la distance entre 2 Tiles est plus que ce que l'unit peut sauter par-dessus
        if ((Mathf.Abs(from.height - to.height) > jumpHeight))
            return false;
        //Skip si la Tile est occupée par un ennemi
        if (to.content != null)
            return false;
        return base.ExpandSearch(from, to);
    }

    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);
        //Construit une liste de points rpz les unités de la Tile de départ à celle d'arrivée
        List<Tile> targets = new List<Tile>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prev;
        }
        //Bouge d'un point à un autre à la suite
        for (int i = 1; i < targets.Count; ++i)
        {
            Tile from = targets[i - 1];
            Tile to = targets[i];
            Direction dir = from.GetDirection(to);
            if (unit.dir != dir)
                yield return StartCoroutine(Turn(dir));
            if (from.height == to.height)
                yield return StartCoroutine(Walk(to));
            else
                yield return StartCoroutine(Jump(to));
        }
        yield return null;
    }
    IEnumerator Walk(Tile target)
    {
        Tweener tweener = transform.MoveTo(target.center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }
    IEnumerator Jump(Tile to)
    {
        Tweener tweener = transform.MoveTo(to.center, 0.5f, EasingEquations.Linear);
        Tweener t2 = jumper.MoveToLocal(new Vector3(0, Tile.stepHeight * 2f, 0), tweener.easingControl.duration / 2f, EasingEquations.EaseOutQuad);
        t2.easingControl.loopCount = 1;
        t2.easingControl.loopType = EasingControl.LoopType.PingPong;
        while (tweener != null)
            yield return null;
    }
}
