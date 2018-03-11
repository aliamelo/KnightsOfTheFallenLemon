using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public const float stepHeight = 0.25f;
    public GameObject content;

    //HideInIspector pour cacher ces fields dans Unity
    [HideInInspector] public Tile prev; //Sauvegarde la Tile traversée pour arriver sur l'actuelle
    [HideInInspector] public int distance; //Nombre de Tile pour atteindre le point

    //Garder position + taille tile 
    public Point pos;
    public int height;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Pour mettre d'autres objets sur la Tile (en son centre)
    public Vector3 center
    {
        get { return new Vector3(pos.x, height * stepHeight, pos.y); }
    }

    //Change l'apparence de la Tile(Unity) si besoin
    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    //Pour changer la taille des tiles random dans la génération de map
    public void Grow()
    {
        height++;
        Match();
    }
    public void Shrink()
    {
        height--;
        Match();
    }

    //Reecrit pour que Tile reste Vector3
    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }
    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
