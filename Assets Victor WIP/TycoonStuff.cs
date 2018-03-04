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
    [SerializeField] List<Character> Team = new List<Character>();
    //[SerializeField] GameObject characterprefab;
    bool play;
    public bool doWindow0 = false;
    public bool doWindow1 = false;
    public bool doWindow2 = false;
    public int choice = -1;
    public string cname = "";
    public int nbProfiles = 1;
    public GameObject[] hireList;
    Character.Characterclass type;

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

    float getIncome(List<Character> team)
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
            //Debug.Log("" + chara.c);
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

    void Hire(int windowID)
    {
        
        cname = GUILayout.TextField(cname);
        string[] classes = {"Intérim", "Stagiaire", "Secrétaire", "Ingénieur", "Manageur", "Technicien de surface", "Agent de sécurité", "Comptable"};
        choice = GUILayout.SelectionGrid(choice, classes, 2);
        type = (Character.Characterclass)choice;
        nbProfiles = (int) GUILayout.HorizontalSlider(nbProfiles, 1, 10);
        //bool create = GUILayout.Button("Create");
        if (cname != "" && GUILayout.Button("Hire!")) 
            {
            hireList = GenerateHireList(nbProfiles, type);
            doWindow2 = true;                       
            choice = -1;
            doWindow0 = false;
            }     
    }

    void ChoicesDisplay(int WindowID)
    {
        foreach (GameObject candidateObj in hireList)
        {
            Character candidate = candidateObj.GetComponent<Character>();
            //
            // BoxContent += "name : " + candidate.GetComponent<Character>().cname + "\n";
            string BoxContent = "";
            BoxContent += "Life : " + candidate.life + "\n";
            BoxContent += "Atk : " + candidate.atk + "\n";
            BoxContent += "Def : " + candidate.def + "\n";
            BoxContent += "MAtk : " + candidate.matk + "\n";
            BoxContent += "MDef : " + candidate.mdef + "\n";
            BoxContent += "Spd : " + candidate.spd + "\n";
            //Debug.Log(BoxContent);
            if (GUILayout.Button(BoxContent))
            {
                GameObject c = new GameObject();
                c.AddComponent<Character>();               
                c.GetComponent<Character>().c = type;
                c.GetComponent<Character>().name = cname;
                c.GetComponent<Character>().cname = cname;
                c.GetComponent<Character>().life = candidate.life;
                c.GetComponent<Character>().atk = candidate.atk;
                c.GetComponent<Character>().def = candidate.def;
                c.GetComponent<Character>().matk = candidate.matk;
                c.GetComponent<Character>().mdef = candidate.mdef;
                c.GetComponent<Character>().spd = candidate.spd;
                Team.Add(c.GetComponent<Character>());
                foreach (GameObject o in hireList)
                    Destroy(o);
                doWindow2 = !doWindow2;             
            }
            // Debug.Log(BoxContent);
        }
    }

    void Fire(int windowID)
    {
        for (int i = 1; i < Team.Count; i++)
        {
            if (GUILayout.Button("" + Team[i].cname))
            {
                DestroyObject(GameObject.Find(Team[i].name));
                Team.RemoveAt(i);
            }
            
        }
    }
    void OnGUI()
    {
        Rect window = new Rect(new Vector2(Screen.width / 2 - 250, Screen.height / 2 - 250), new Vector2(500, 100));
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
        if (Team.Count < baseCapacity[baseLevel])
        {
            if (GUILayout.Button("Hire"))
            {
                doWindow0 = !doWindow0;
                cname = "";
            }
            if (doWindow0)
            {
                GUILayout.Window(0, window, Hire, "Hire new Character");                
            }
        }
        if (Team.Count > 1)
        {
            if (GUILayout.Button("Fire"))
                doWindow1 = !doWindow1;
            if (doWindow1)
            {
                GUILayout.Window(1, window, Fire, "Fire Character");
            }
        } else doWindow1 = false;
        if (doWindow2)
        {
            GUILayout.Window(2, window, ChoicesDisplay, "Hire List");
        }        


    }

    GameObject[] GenerateHireList(int nbChara, Character.Characterclass c)
    {
        GameObject[] hireList = new GameObject[nbChara];
        GameObject chara = new GameObject();
        chara.AddComponent<Character>();
        chara.GetComponent<Character>().c = c;
        //chara.GetComponent<Character>().name = cname;
        chara.GetComponent<Character>().cname = cname;
        for (int i = 0; i < nbChara; i++)
        {
            //GameObject chara = new GameObject();
            GameObject instance = Instantiate(chara);
            hireList[i] = instance/*.GetComponent<Character>()*/;
        }
        return hireList;
    }




    // Use this for initialization
    void Start()
    {
        GameObject c = new GameObject();
        c.AddComponent<Character>();
        c.GetComponent<Character>().c = Character.Characterclass.PDG;
        c.GetComponent<Character>().name = "Player 1";
        c.GetComponent<Character>().cname = "Player 1";
        Team.Add(c.GetComponent<Character>());
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    /*switch (type)
                {
                    case Character.Characterclass.COUNT:
                        c.AddComponent<Compt>();
                        c.GetComponent<Character>().c = 
                        // instance.AddComponent<Character>();                
                        break;
                    case Character.Characterclass.GUARD:
                        c.AddComponent<Secur>();
                        //  instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.ING:
                        c.AddComponent<Inge>();
                        //  instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.INTER:
                        c.AddComponent<Interimaire>();
                        // instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.MAN:
                        c.AddComponent<Man>();
                        // instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.PDG:
                        c.AddComponent<PDG>();
                        // instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.SEC:
                        c.AddComponent<Secretaire>();
                        // instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.STAG:
                        c.AddComponent<Stagiaire>();
                        //  instance.AddComponent<Character>();
                        break;
                    case Character.Characterclass.TECH:
                        c.AddComponent<Tech>();
                        // instance.AddComponent<Character>();
                        break;
                    default:
                        c.AddComponent<Character>();
                        break;
                }*/
}
