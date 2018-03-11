using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Direction dir;

    public void Place(Tile target) //Place l'unité
    {
        //Vérifie que l'ancienne location de Tile ne pointe pas sur l'unité
        if (tile != null && tile.content == gameObject)
            tile.content = null;
        
        // Lie Unité et Tile
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    public void Match() //Met l'unité au centre de la Tile
    {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}
