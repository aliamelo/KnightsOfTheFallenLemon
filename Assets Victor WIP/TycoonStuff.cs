using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TycoonStuff : MonoBehaviour
{
    public int[] baseCapacity = { 2, 10, 20, 30 };
    public int[] upgradeCost = { 25000, 500000, 2500000 };
    public int nbEmployee = 1;
    public float money = 10000;
    public int baseLevel = 0;
    public float income = 0;
    [SerializeField] Character[] Team = new Character[2];
    [SerializeField] GameObject characterprefab;
    bool play;

    /* c.AddComponent<Ing>();
    
     c2.AddComponent<Stagiaire>();
         Team[0] = c;
         Team[1] = c2*/


    void upgradeBase()
    {
        if (upgradeCost[baseLevel] < money)
        {
            money -= upgradeCost[baseLevel];
            baseLevel++;
         // newteam =  
        }
    }

    float getIncome(Character[] team)
    {
        float income = 0;
        int ingCount = 0;
        int stagCount = 0;
        int manCount = 0;
        int secCount = 0;
        int comptCount = 0;

        foreach(Character chara in team)
        {

            if (chara == null)
                break;
            Debug.Log("" + chara.c);
            switch(chara.c)
            {
                case Character.Characterclass.ING:
                    ingCount++;
                    break;
                case Character.Characterclass.STAG:
                    stagCount++;
                    break;
                case Character.Characterclass.MAN:
                    manCount++;
                    break;
                case Character.Characterclass.SEC:
                    secCount++;
                    break;
                case Character.Characterclass.COUNT:
                    comptCount++;
                    break;
                default:
                    break;
            }
        }
        income = (ingCount * 2000 + stagCount * 500 + (manCount < nbEmployee / 5 ? -1000 : 0) + (nbEmployee > 10 && comptCount == 0 ? -2000 : 0)) * (1 + secCount * 1.1f);
        return income;
    }

    void updateMoney()
    {
        income = getIncome(Team);
        money += income;
    }

    void OnGUI()
    {
        GUILayout.Box("" + money);
        GUILayout.Box("Base Level : " + (baseLevel + 1));
        GUILayout.Box("Income : " + income);       
        if (GUILayout.Button("Next Month"))
        {
            updateMoney();
        }
        if (GUILayout.Button("Upgrade Base"))
        {
            upgradeBase();
        }
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Team = 
        
    }
}
