using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JSONSTUFF;

public class TycoonStuff : MonoBehaviour
{
    public int[] baseCapacity = { 2, 10, 20, 30 };
    public int[] upgradeCost = { 25000, 500000, 2500000 };
    public int nbEmployee = 1;
    public int money = 10000;
    public int baseLevel = 0;
    public float income = 0;
    [SerializeField] List<Character> Team = new List<Character>();
    bool play;
    public bool doWindow0 = false;
    public bool doWindow1 = false;
    public bool doWindow2 = false;
    public int choice = -1;
    public string cname = "";
    public int nbProfiles = 1;
    public Character[] hireList;
    public bool startscreen = false;
    public bool newplayer = false;
    public bool load = false;
    public string trash = "";
    private JSONElement State;
    Character.Characterclass type;


    void upgradeBase()
    {
        if (upgradeCost[baseLevel] < money)
        {
            money -= upgradeCost[baseLevel];
            baseLevel++;
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
        money += (int)income;
    }

    void Hire(int windowID)
    {        
        cname = GUILayout.TextField(cname);
        string[] classes = {"Intérim", "Stagiaire", "Secrétaire", "Ingénieur", "Manageur", "Technicien de surface", "Agent de sécurité", "Comptable"};
        choice = GUILayout.SelectionGrid(choice, classes, 2);
        type = (Character.Characterclass)choice;
        nbProfiles = (int) GUILayout.HorizontalSlider(nbProfiles, 1, 10);
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
        foreach (Character candidate in hireList)
        {
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
                Character c = new Character(type, cname);                
                c.c = type;
                c.life = candidate.life;
                c.atk = candidate.atk;
                c.def = candidate.def;
                c.matk = candidate.matk;
                c.mdef = candidate.mdef;
                c.spd = candidate.spd;
                Team.Add(c);
                SaveTeam();
                doWindow2 = !doWindow2;             
            }
        }
    }

    void Fire(int windowID)
    {
        for (int i = 1; i < Team.Count; i++)
        {
            if (GUILayout.Button("" + Team[i].cname))
            {                
                Team.RemoveAt(i);
                SaveTeam();
            }
            
        }
    }

    void StartScreen(int windowID)
    {
        if(GUILayout.Button("New Player"))
        {
            newplayer = true;
            startscreen = false;
        }
        if(GUILayout.Button("Load Team"))
        {
            load = true;
            startscreen = false;
        }
        
    }

   void ParseTeam(JSONElement T)
    {
        int i = 0;
        if (T.key == null)
            throw new System.Exception("HELL NAH BRO");
        foreach (string name in T.key)
        {
            Character chara;
            switch (JSON.SearchJSON(T.data[i], "class").string_value)
            {
                case "INTER":
                    chara = new Character(Character.Characterclass.INTER, name);
                    break;
                case "STAG":
                    chara = new Character(Character.Characterclass.STAG, name);
                    break;
                case "SEC":
                    chara = new Character(Character.Characterclass.SEC, name);
                    break;
                case "ING":
                    chara = new Character(Character.Characterclass.ING, name);
                    break;
                case "MAN":
                    chara = new Character(Character.Characterclass.MAN, name);
                    break;
                case "TECH":
                    chara = new Character(Character.Characterclass.TECH, name);
                    break;
                case "GUARD":
                    chara = new Character(Character.Characterclass.GUARD, name);
                    break;
                case "COUNT":
                    chara = new Character(Character.Characterclass.COUNT, name);
                    break;
                default:
                    chara = new Character(Character.Characterclass.PDG, name);
                    break;
            }
                        
            chara.lvl = JSON.SearchJSON(T.data[i], "lvl").int_value;
            chara.life = JSON.SearchJSON(T.data[i], "life").int_value;
            chara.atk = JSON.SearchJSON(T.data[i], "atk").int_value;
            chara.def = JSON.SearchJSON(T.data[i], "def").int_value;
            chara.matk = JSON.SearchJSON(T.data[i], "matk").int_value;
            chara.mdef = JSON.SearchJSON(T.data[i], "mdef").int_value;
            chara.spd = JSON.SearchJSON(T.data[i], "spd").int_value;
            Team.Add(chara);
            i++;
        }
    }

void NewPlayer(int windowID)
{
    cname = GUILayout.TextField(cname);
    if (cname != "" && GUILayout.Button("Start"))
    {
        Character c = new Character(Character.Characterclass.PDG, cname);        
        Team.Add(c);
        SaveTeam();
        newplayer = false;
    }
}

void Load(int windowID)
    {
        trash = GUILayout.TextField(trash);
        string path = ".\\" + trash;
        if (GUILayout.Button("Load Team") && trash != "")
        {
            if(File.Exists(path))
            {
                JSONElement T = JSON.ParseJSONFile(path);
                // File.WriteAllText(path + "0", JSON.PrintJSON(T));
                if (T != null)
                {
                    ParseTeam(JSON.SearchJSON(T, "Team"));
                    money = JSON.SearchJSON(T, "Money").int_value;
                    Debug.Log(JSON.SearchJSON(T, "Money").int_value);
                    baseLevel = JSON.SearchJSON(T, "Base Level").int_value;
                    Debug.Log(JSON.SearchJSON(T, "Base Level").int_value);
                }
                load = false;
            }
        }
    }

    void OnGUI()
    {
        Rect window = new Rect(new Vector2(Screen.width / 2 - 250, Screen.height / 2 - 250), new Vector2(500, 100));
        if (!startscreen)
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
            }
            else doWindow1 = false;
            if (doWindow2)
            {
                GUILayout.Window(2, window, ChoicesDisplay, "Hire List");
            }
        }     
        if (newplayer)
        {
             GUILayout.Window(4, window, NewPlayer, "Create New Player");
        }
        if (load)
        {
            GUILayout.Window(5, window, Load, "Load Team");
        }
        if (startscreen)
        {
            GUILayout.Window(3, window, StartScreen, "Startscreen");
        }

    }

    public void SaveTeam()
    {
        JSONElement State = new JSONElement(JSONElement.JSONType.DIC);
        State.Add("Base Level", new JSONElement(baseLevel));
        State.Add("Money", new JSONElement(money));
        string path = "./" + Team[0].cname;
        File.Delete(path);
        JSONElement TeamSpecs = new JSONElement(JSONElement.JSONType.DIC);
        foreach (Character c in Team)
        {
            JSONElement CharaSpecs = new JSONElement(JSONElement.JSONType.DIC);         
            CharaSpecs.Add("name", new JSONElement(c.cname));
            CharaSpecs.Add("lvl", new JSONElement(c.lvl));
            CharaSpecs.Add("class", new JSONElement("" + c.c));
            CharaSpecs.Add("life", new JSONElement(c.life));
            CharaSpecs.Add("atk", new JSONElement(c.atk));
            CharaSpecs.Add("def", new JSONElement(c.def));
            CharaSpecs.Add("matk", new JSONElement(c.matk));
            CharaSpecs.Add("mdef", new JSONElement(c.mdef));
            CharaSpecs.Add("spd", new JSONElement(c.spd));
            TeamSpecs.Add(c.cname, CharaSpecs);
        }
        State.Add("Team", TeamSpecs);
        File.WriteAllText(path, JSON.PrintJSON(State));
    }

    Character[] GenerateHireList(int nbChara, Character.Characterclass c)
    {
        Character[] hireList = new Character[nbChara];
        GameObject chara = new GameObject();
        for (int i = 0; i < nbChara; i++)
        {
            hireList[i] = new Character(c, cname);
        }
        return hireList;
    }




    // Use this for initialization
    void Start()
    {
        startscreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

}
