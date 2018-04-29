using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taser : Items {
    protected int Cooldown;

    Taser(string name, int lvl = 0, int Cooldown = 5) : base(ItemType.SUPPORT, name, lvl)
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
}
