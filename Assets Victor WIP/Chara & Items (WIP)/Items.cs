using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {

    public enum ItemType
    {
        WEAPON,
        SUPPORT
    };

    public string iname;
    protected int lvl;
    protected ItemType type;
  //  int[] stats = new int[1];
    public Items(ItemType t, string name, int lvl = 0)
    {
        this.type = t;
        this.iname = name;
        this.lvl = 0;
    }

    public bool Upgrade()
    {
        if (this.lvl < 2)
        {
            this.lvl++;
            UpdateStats();
            return true;
        }
        else return false;
    }

    public virtual void UpdateStats()
    {  
              
    }

    public virtual void UseItem(Character c)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
