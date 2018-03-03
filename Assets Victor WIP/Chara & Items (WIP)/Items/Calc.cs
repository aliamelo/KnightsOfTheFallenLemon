using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : Items
{

    int dmg;
    Calc(string name, int lvl, int dmg): base (ItemType.WEAPON, name, lvl)
    {
        this.dmg = dmg;
    }

    public override void UpdateStats()
    {
        dmg += 5;
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
