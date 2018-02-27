using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech : Character
{
    protected int[] stats = { 15, 45, 5, 0, 20, 45 };
   
    Tech(string name, int lvl = 0) : base(Character.Characterclass.TECH, name, lvl)
    {
        this.name = name;
        for (int i = 0; i < 6; i++)
        {
            stats[i] += Random.Range(-2, 2);
        }
        base.life = stats[0];
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
