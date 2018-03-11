using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range;
    public int jumpHeight;
    protected Unit unit;
    protected Transform jumper;

    //Pour l'animation d'un passage à l'autre
    public abstract IEnumerator Traverse(Tile tile);

    //Asignement des références
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        jumper = transform.Find("Jumper");
    }

    //Liste de Tiles atteignables
    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }

    //Peut être réécrite => compare la distance parcourue par rapport à la portée du perso
    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }

    //Peut être réécrite => enlève les Tiles boquées(content)
    protected virtual void Filter(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    //Animation pour tourner le personnage vers une certaine direction
    protected virtual IEnumerator Turn(Direction dir)
    {
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        //Pendant la rotation entre le Nord et l'Ouest, cas à part => faire comme si l'unité se tournait de la façon la plus efficace
        if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
            t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
            t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        unit.dir = dir;

        while (t != null)
            yield return null;
    }
}
