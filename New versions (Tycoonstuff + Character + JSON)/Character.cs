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
    public int xp;
    public int[] xpsteps = { 2000, 40000, 800000 };
    private float[] step;
    public List<Items> inventory;
    protected int[] stats;
    protected int[] interstats;
    public int atk;
    public int def;
    public int matk;
    public int mdef;
    public int spd;
    public int salary;

    public Character(Characterclass c, string name, int lvl = 0)
    {
        inventory = new List<Items>(4);
        switch (c)
        {
            case Character.Characterclass.COUNT:
                stats = new int[] { 30, 30, 40, 30, 40, 100 };
                step = new float[] { 1.3f, 1.4f, 1.2f, 1.3f, 1.4f, 1.3f };
                salary = 2500;
               // Calc cal = new Calc("TI 83+");
              //  inventory[0] = cal;
                break;
            case Character.Characterclass.GUARD:
                stats = new int[] { 50, 20, 50, 0, 30, 100 };
                step = new float[] { 1.3f, 2.1f, 2.1f, 0, 1.3f, 1.2f };
                salary = 1500;
              //  Stick st = new Stick("Basic stick");
              //  inventory[0] = st;
                break;
            case Character.Characterclass.ING:
                salary = 1800;
                stats = new int[] { 30, 10, 30, 50, 40, 80 };
                step = new float[] { 1.1f, 1.3f, 1.3f, 1.8f, 1.5f, 1.5f };
                // Computer co = new Computer("Raspberry Pi 0");
                break;
            case Character.Characterclass.INTER:
                salary = 1000;
                stats = new int[] { 20, 20, 20, 20, 20, 100 };
                step = new float[] { 1.3f, 1.3f, 1.3f, 1.3f, 1.3f, 1.3f };
                break;
            case Character.Characterclass.MAN:
                salary = 1800;
                stats = new int[] { 20, 40, 20, 40, 20, 110 };
                step = new float[] { 1.8f, 1.4f, 1.4f, 1.7f, 1.4f, 1.5f };
                // Money m = new Money("just a little bit");
                // inventory[0] = m;
                break;
            case Character.Characterclass.PDG:
                salary = 0;
                stats = new int[] { 20, 10, 20, 10, 20, 120 };
                step = new float[] { 1.3f, 1.4f, 1.4f, 1.3f, 1.4f, 1.5f };
                // Money m2 = new Money("just a little bit");
                // inventory[0] = m2;
                break;
            case Character.Characterclass.SEC:
                salary = 1200;
                stats = new int[] { 30, 30, 30, 30, 30, 110 };
                step = new float[] { 1.2f, 1.2f, 1.2f, 1.2f, 1.2f, 1.3f };
                //  Stapler s = new Stapler("Stapler of doom", 0, 5);
                //  inventory[0] = s;
                break;
            case Character.Characterclass.STAG:
                salary = 800;
                stats = new int[] { 20, 20, 20, 20, 20, 90 };
                step = new float[] { 1.3f, 1.3f, 1.3f, 1.3f, 1.3f, 1.2f };
                //  Coffee cof = new Coffee("Nespresso");
                //  inventory[0] = cof;
                break;
            case Character.Characterclass.TECH:
                salary = 1200;
                stats = new int[] { 20, 50, 10, 0, 10, 80 };
                step = new float[] { 2.2f, 1.1f, 1.2f, 0, 1.1f, 1.6f };
                //  Broom b = new Broom("Nimbus 2000");
                //  inventory[0] = b;
                break;
            default:
                stats = new int[] { 25, 25, 25, 25, 25, 25 };
                break;
        }
        for (int i = 0; i < 6; i++)
        {
            stats[i] += Random.Range(-2, 2);
            stats[i] *= (int) Mathf.Pow(2, lvl);
            if (stats[i] < 0)
            {
                stats[i] = 0;
            }

        }
        salary += Random.Range(-200, +150);
        salary *= lvl + 1;
        if (salary < 0)
            salary = 0;
        life = stats[0];
        maxLife = life;
        this.lvl = 1;
        atk = stats[1];
        def = stats[2];
        matk = stats[3];
        mdef = stats[4];
        spd = stats[5];

        this.c = c;
        this.lvl = lvl;
        this.cname = name;
        interstats = stats;
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

    public void lvlUp()
    {
        life += (int)((step[0] + 1) * Life);       
        atk = (int)((step[1] + 1) * Atk);
        def = (int)((step[2] + 1) * Def);
        matk = (int)((step[3] + 1) * Matk);
        mdef = (int)((step[4] + 1) * Mdef);
        spd = (int)((step[5] + 1) * Spd);
        maxLife = life;
        Life = life;
        Atk = atk;
        Def = def;
        Matk = matk;
        Mdef = mdef;
        Spd = spd;
    }

    public void updateStats()
    {
        interstats[0] = (int)(stats[0] * (1 + xp / xpsteps[lvl] * 100 * (step[0] + 1) / 2));
        interstats[1] = (int)(stats[1] * (1 + xp / xpsteps[lvl] * 100 * (step[1] + 1) / 2));
        interstats[2] = (int)(stats[2] * (1 + xp / xpsteps[lvl] * 100 * (step[2] + 1) / 2));
        interstats[3] = (int)(stats[3] * (1 + xp / xpsteps[lvl] * 100 * (step[3] + 1) / 2));
        interstats[4] = (int)(stats[4] * (1 + xp / xpsteps[lvl] * 100 * (step[4] + 1) / 2));
        interstats[5] = (int)(stats[5] * (1 + xp / xpsteps[lvl] * 100 * (step[5] + 1) / 2));
        life = interstats[0];
        maxLife = life;
        atk = interstats[1];
        def = interstats[2];
        matk = interstats[3];
        mdef = interstats[4];
        spd = interstats[5];
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
