using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;

    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    private Point[] dirs = new Point[4]
    {
      new Point(0, 1),
      new Point(0, -1),
      new Point(1, 0),
      new Point(-1, 0)
    };

    //Pour changer la couleur des Tiles sur lesquelles le joueur peut se déplacer
    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);

    //retourne une liste de Tiles qui tombent sous certains critères(func, bool pour autoriser ou non le mouvement)
    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);
        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>(); //Tiles à vérifier dans le futur
        Queue<Tile> checkNow = new Queue<Tile>(); //Tiles à vérifier maintenant
        start.distance = 0;
        checkNow.Enqueue(start);
        while (checkNow.Count > 0) 
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]); 
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }
            if (checkNow.Count == 0) //Echange de queue
                SwapReference(ref checkNow, ref checkNext);
        }
        return retValue;
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    //Instancie toutes les Tiles de la map
    public void Load(LevelData data)
    {
        for(int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    //Clear les Tiles pour pouvoir chercher un path
    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    //Change la couleur des Tiles
    public void SelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    //Remet la couleur normale
    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }
}
