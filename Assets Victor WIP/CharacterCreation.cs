using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {
    public GameObject characterprefab;
    //[SerializeField] GameObject characterprefab;
    public GameObject tileSelectionIndicatorPrefab;
    //[SerializeField] GameObject tileSelectionIndicatorPrefab;
    public new string name;
    //[SerializeField] new string name;
    public int Lvl = 0;
    //[SerializeField] int Lvl = 0;
    public Character.Characterclass type;
    //[SerializeField] Character.Characterclass type;
    public Point pos;
    //[SerializeField] Point pos;
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
       /* c.cname = name;
        c.lvl = Lvl;
        switch (type)
        {
            case Character.Characterclass.COUNT:
                c.Life = 20;
                c.Atk = 20;
                c.Def = 35;
                c.Matk = 20;
                c.Mdef = 35;
                c.Spd = 10;                
                break; 
            case Character.Characterclass.GUARD:
                c.Life = 45;
                c.Atk = 15;
                c.Def = 45;
                c.Matk = 0;
                c.Mdef = 15;
                c.Spd = 10;
                break;
            case Character.Characterclass.ING:
               // c.stats = new int[] { 25, 15, 5, 40, 35, 35 };
                c.Life = 25;
                c.Atk = 15;
                c.Def = 5;
                c.Matk = 40;
                c.Mdef = 35;
                c.Spd = 35;
                break;
            case Character.Characterclass.INTER:
                // c.stats = new int[] { 15, 15, 15, 15, 15, 15 };
                c.Life = 15;
                c.Atk = 15;
                c.Def = 15;
                c.Matk = 15;
                c.Mdef = 15;
                c.Spd = 15;
                break;
            case Character.Characterclass.MAN:
                // c.stats = new int[] { 10, 35, 10, 35, 10, 20 };
                c.Life = 10;
                c.Atk = 35;
                c.Def = 10;
                c.Matk = 35;
                c.Mdef = 10;
                c.Spd = 20;
                break;
            case Character.Characterclass.PDG:
                // c.stats = new int[] { 10, 5, 10, 5, 10, 10 };
                c.Life = 10;
                c.Atk = 5;
                c.Def = 10;
                c.Matk = 5;
                c.Mdef = 10;
                c.Spd = 10;
                break;
            case Character.Characterclass.SEC:
                //c.stats = new int[] { 10, 10, 10, 10, 10, 40 };
                c.Life = 10;
                c.Atk = 10;
                c.Def = 10;
                c.Matk = 10;
                c.Mdef = 10;
                c.Spd = 40;
                c.inventory = new Items[4];
                Stapler s = new Stapler("stapler of doom", 0, 5);
                c.inventory[0] = s;
                break;
            case Character.Characterclass.STAG:
                // c.stats = new int[] { 20, 20, 20, 20, 20, 20 };
                c.Life = 20;
                c.Atk = 20;
                c.Def = 20;
                c.Matk = 20;
                c.Mdef = 20;
                c.Spd = 20;
                break;
            case Character.Characterclass.TECH:
                // c.stats = new int[] { 15, 45, 5, 0, 20, 45 };
                c.Life = 15;
                c.Atk = 45;
                c.Def = 5;
                c.Matk = 0;
                c.Mdef = 20;
                c.Spd = 45;
                break;
            default:
                // c.stats = new int[] { 25, 25, 25, 25, 25, 25 };
                c.Life = 25;
                c.Atk = 25;
                c.Def = 25;
                c.Matk = 25;
                c.Mdef = 25;
                c.Spd = 25;
                break;
        }
        c.c = type;
        c.life = c.Life;*/
        c.transform.localPosition = new Vector3(pos.x, 0, pos.y);
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
