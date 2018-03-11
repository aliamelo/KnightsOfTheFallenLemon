using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn  
{
    public Unit actor; //Hollywood
    public bool hasUnitMoved; //Si l'unité a bougé
    public bool hasUnitActed; //Si l'unité a joué
    public bool lockMove; //Pour bloquer le mouvement

    Tile startTile; //La tile pour démarrer

    Direction startDir; //La direction pour démarrer

    public void Change(Unit current)
    {
        actor = current;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.tile;
        startDir = actor.dir;
    }

    public void UndoMove()
    {
        hasUnitMoved = false;
        actor.Place(startTile);
        actor.dir = startDir;
        actor.Match();
    }
}
