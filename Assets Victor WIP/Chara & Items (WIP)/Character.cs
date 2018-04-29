using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
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
    public Items[] inventory;
    protected int[] stats;
    public int atk;
    public int def;
    public int matk;
    public int mdef;
    public int spd;
    public int salary;

    public Character(Characterclass c, string name, int lvl = 0)
    {
        inventory = new Items[4];
        switch (c)
        {
            case Character.Characterclass.COUNT:
                stats = new int[] { 20, 20, 35, 20, 35, 10 };
                Calc c = new Calc("TI 83+");
                inventory[0] = c;
                break;
            case Character.Characterclass.GUARD:
                stats = new int[] { 45, 15, 45, 0, 15, 10 };
                Stick st = new Stick("Basic stick");
                inventory[0] = st;
                break;
            case Character.Characterclass.ING:
                stats = new int[] { 25, 15, 5, 40, 35, 35 };
                Computer co = new Computer("Raspberry Pi 0");
                break;
            case Character.Characterclass.INTER:
                stats = new int[] { 15, 15, 15, 15, 15, 15 };
                break;
            case Character.Characterclass.MAN:
                stats = new int[] { 10, 35, 10, 35, 10, 20 };
                Money m = new Money("just a little bit");
                inventory[0] = m;
                break;
            case Character.Characterclass.PDG:
                stats = new int[] { 10, 5, 10, 5, 10, 10 };
                Money m = new Money("just a little bit");
                inventory[0] = m;
                break;
            case Character.Characterclass.SEC:
                stats = new int[] { 10, 10, 10, 10, 10, 40 };
                Stapler s = new Stapler("Stapler of doom", 0, 5);
                inventory[0] = s;
                break;
            case Character.Characterclass.STAG:
                stats = new int[] { 20, 20, 20, 20, 20, 20 };
                Coffee cof = new Coffee("Nespresso");
                inventory[0] = cof;
                break;
            case Character.Characterclass.TECH:
                stats = new int[] { 15, 45, 5, 0, 20, 45 };
                Broom b = new Broom("Nimbus 2000");
                inventory[0] = b;
                break;
            default:
                stats = new int[] { 25, 25, 25, 25, 25, 25 };
                break;
        }
        for (int i = 0; i < 6; i++)
        {
            stats[i] += Random.Range(-2, 2);
            if (stats[i] < 0)
            {
                stats[i] = 0;
            }

        }
        life = stats[0];
        maxLife = life;
        this.lvl = 1;
        atk = stats[1];
        def = stats[2];
        matk = stats[3];
        mdef = stats[4];
        spd = stats[5];



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

    public void ForceStats(int[] stats)
    {
        this.stats = stats;
        life = Life;
        maxLife = life;
        atk = Atk;
        def = Def;
        matk = Matk;
        mdef = Mdef;
        spd = Spd;
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
}
