using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taser : Items {
    protected int Cooldown;

    Coffee(string name, int lvl, int Cooldown = 5) : base(ItemType.SUPPORT, name, lvl)
    {
        this.Cooldown = Cooldown;
    }

    public override void UpdateStats()
    {
        Cooldown--;
    }

    public override void UseItem(Character target)
    {
        target.ApplyEffect('s', 1);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
