using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {
    [SerializeField] GameObject characterprefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;
    [SerializeField] new string name;
    [SerializeField] int Lvl = 0;
    [SerializeField] Character.Characterclass type;    
    [SerializeField] Point pos;
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    Character make()
    {
        GameObject instance = Instantiate(characterprefab);
        instance.transform.parent = transform;
        switch (type)
        {
            case Character.Characterclass.COUNT:
                instance.AddComponent<Compt>();
               // instance.AddComponent<Character>();                
                return instance.GetComponent<Compt>();
            case Character.Characterclass.GUARD:
                instance.AddComponent<Secur>();
              //  instance.AddComponent<Character>();
                return instance.GetComponent<Secur>();
            case Character.Characterclass.ING:
                instance.AddComponent<Inge>();
              //  instance.AddComponent<Character>();
                return instance.GetComponent<Inge>();
            case Character.Characterclass.INTER:
                instance.AddComponent<Interimaire>();
               // instance.AddComponent<Character>();
                return instance.GetComponent<Interimaire>();
            case Character.Characterclass.MAN:
                instance.AddComponent<Man>();
               // instance.AddComponent<Character>();
                return instance.GetComponent<Man>();
            case Character.Characterclass.PDG:
                instance.AddComponent<PDG>();
               // instance.AddComponent<Character>();
                return instance.GetComponent<PDG>();
            case Character.Characterclass.SEC:
                instance.AddComponent<Secretaire>();
               // instance.AddComponent<Character>();
                return instance.GetComponent<Secretaire>();
            case Character.Characterclass.STAG:
                instance.AddComponent<Stagiaire>();
              //  instance.AddComponent<Character>();
                return instance.GetComponent<Stagiaire>();
            case Character.Characterclass.TECH:
                instance.AddComponent<Tech>();
               // instance.AddComponent<Character>();
                return instance.GetComponent<Tech>();
            default:
                instance.AddComponent<Character>();
                return instance.GetComponent<Character>();
        }
    }


   Transform marker
    {
            get
        {
                if (_marker == null)
                {
                    GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                    _marker = instance.transform;
                }
                return _marker;
            }
        }
        Transform _marker;



        public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    public void create()
    {
        Character c = make();
        c.cname = name;
        c.lvl = Lvl;
        switch (type)
        {
            case Character.Characterclass.COUNT:
                c.stats = new int[] { 20, 20, 35, 20, 35, 10 };                
                break; 
            case Character.Characterclass.GUARD:
                c.stats = new int[] { 45, 15, 45, 0, 15, 10 };
                break;
            case Character.Characterclass.ING:
                c.stats = new int[] { 25, 15, 5, 40, 35, 35 };
                break;
            case Character.Characterclass.INTER:
                c.stats = new int[] { 15, 15, 15, 15, 15, 15 };
                break;
            case Character.Characterclass.MAN:
                c.stats = new int[] { 10, 35, 10, 35, 10, 20 };
                break;
            case Character.Characterclass.PDG:
                c.stats = new int[] { 10, 5, 10, 5, 10, 10 };
                break;
            case Character.Characterclass.SEC:
                c.stats = new int[] { 10, 10, 10, 10, 10, 40 };
                c.inventory = new Items[4];
                Stapler s = new Stapler("stapler of doom", 0, 5);
                c.inventory[0] = s;
                break;
            case Character.Characterclass.STAG:
                c.stats = new int[] { 20, 20, 20, 20, 20, 20 };
                break;
            case Character.Characterclass.TECH:
                c.stats = new int[] { 15, 45, 5, 0, 20, 45 };
                break;
            default:
                c.stats = new int[] { 25, 25, 25, 25, 25, 25 };
                break;
        }
        c.c = type;
        c.life = c.stats[0];
        c.transform.localPosition = new Vector3(pos.x, 0, pos.y);
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
