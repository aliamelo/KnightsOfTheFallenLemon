using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech : Character
{
    int[] istats = { 15, 45, 5, 0, 20, 45 };

    public Tech(string name, int lvl = 0) : base(Character.Characterclass.TECH, name, lvl)
    {
        this.name = name;
        for (int i = 0; i < 6; i++)
        {
            istats[i] += Random.Range(-2, 2);
        }
        base.life = istats[0];
    }


    // Use this for initialization
    void Start()
    {
        //Tech tech = new Tech(name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
