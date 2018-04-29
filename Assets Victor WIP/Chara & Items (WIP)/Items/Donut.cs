using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : Items {

    int boost;
    int dam;
    Donut(string name, int lvl = 0, int boost = 10, int dam = 5): base (ItemType.SUPPORT, name, lvl)
    {
        this.boost = boost;
        this.dam = dam;
    }

    public override void UpdateStats()
    {
        boost += 5;
        dam--;
    }

    public override void UseItem(Character target)
    {
        int[] statboost = { 0, this.boost, this.boost, this.boost, this.boost, 0 };
        target.BoostStats(statboost);
        if (target.Class != Character.Characterclass.GUARD)
            target.takeDmg(dam);
    }
}
