using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
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
    protected Characterclass c;
    protected int lvl;
    public string name;
    public int life;
    private int maxLife;
    public Items[] inventory = new Items[4];
    protected int[] stats; 
    public Character(Characterclass c, string name, int lvl = 0)
    {
        life = stats[0];
        maxLife = life;
        this.c = c;
        this.lvl = lvl;
        this.name = name;
    }

    public Characterclass Class()
    { get: return c; }

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
        GameObject chara = new GameObject(name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
