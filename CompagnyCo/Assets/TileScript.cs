using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public bool wakable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<TileScript> adjancecyList = new List<TileScript>();

    //pour le BFS (Breadth First Search)
    public bool visited = false;
    public TileScript parent = null;
    public int distance = 0;

    //Pour A* (ennemis)
    public float f = 0;
    public float g = 0;
    public float h = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
	}

    public void Reset()
    {
        adjancecyList.Clear();

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbours(float jumpHeight, TileScript target)
    {
        Reset();
        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }

    public void CheckTile(Vector3 direction, float jumpHeight, TileScript target)
    {
        Vector3 halfExtents = new Vector3(0.25f,(1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach(Collider item in colliders)
        {
            TileScript tile = item.GetComponent<TileScript>();
            if (tile != null && tile.wakable)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjancecyList.Add(tile);
                }
            }
        }
    }
}
