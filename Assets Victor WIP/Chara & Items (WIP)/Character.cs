using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour{
    public enum Characterclass
    {
        INTER,
        STAG,
        SEC,
        ING,
        MAN,
        TECH,
        GUARD,
        COUNT,
        PDG
    };
    public Characterclass c;
    public int lvl;
    public string cname;
    public int life;
    private int maxLife;
    public Items[] inventory = new Items[4];
    public int[] stats = new int[6]; 
    public Character(Characterclass c, string name, int lvl = 0)
    {
        life = Life;
        maxLife = life;
        this.c = c;
        this.lvl = lvl;
        this.cname = name;
    }
    public int Life
    {
        get { return stats[0]; }
        set { stats[0] = value; }
    }
    public int Atk
    {
        get { return stats[1]; }
        set { stats[1] = value; }
    }
    public int Def
    {
        get { return stats[2]; }
        set { stats[2] = value; }
    }
    public int Matk
    {
        get { return stats[3]; }
        set { stats[3] = value; }
    }
    public int Mdef
    {
        get { return stats[4]; }
        set { stats[4] = value; }
    }
    public int Spd
    {
        get { return stats[5]; }
        set { stats[5] = value; }
    }

public Characterclass Class
    { get { return c; } }

    bool attack(Character opponent, int atk, int def)
    {
        int dammage = atk - def;
        if (dammage > 0)
        {
            opponent.takeDmg(dammage);
            return true;
        }
        return false;        
    }

    public void BoostStats(int[] boost)
    {
        for(int i = 1; i < 6; i++)
        {
            stats[i] += boost[i];
        }
    }

    public int takeDmg(int d)
    {
        this.life -= d;
        if (!isAlive())
            this.life = 0;
        return life;
    }

   

    public int Heal(int h)
    {
        life += h;
        if (life > maxLife)
            life = maxLife;
        return life;
    }

    public void ApplyEffect(char effect, int nbTurn)
    {
        switch(effect)
        {
            case 's':
                break;
            default:
                throw new System.Exception("Unknown effect");
        }
    }

    bool isAlive()
    {
        return this.life > 0;
    }

    // Use this for initialization
    void Start () {
        //GameObject chara = new GameObject(name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
