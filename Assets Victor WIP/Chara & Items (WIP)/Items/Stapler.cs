using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stapler : Items
{

    int dmg;
    int range;
    public Stapler(string name, int lvl, int dmg, int range = 2): base (ItemType.WEAPON, name, lvl)
    {
        this.dmg = dmg;
        this.range = range;
    }

    public override void UpdateStats()
    {
        dmg += 5;
        range++;
    }

    public override void UseItem(Character target)
    {
        throw new System.Exception("Special capacities not implemented yet");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
