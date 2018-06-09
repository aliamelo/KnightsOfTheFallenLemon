using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JSONSTUFF;

public class TycoonStuff : MonoBehaviour
{

    public Camera StartOffice;
    public Camera InterOffice;
    public Camera EndOffice;

    public GUISkin customSkin;
    public GUISkin closeSkin;

    public int[] baseCapacity = { 2, 10, 20, 30 };
    public int[] upgradeCost = { 25000, 2500000 };
    public int nbEmployee = 1;
    public int money = 5000;
    public int baseLevel = 0;
    public float income = 0;
    public List<Character> Team = new List<Character>();
    List<Items> TeamInventory = new List<Items>();
    List<Items> UpgradeWaitingList = new List<Items>();
    List<Character> CharaWaitingList = new List<Character>();

    bool play;
    public bool doWindow0 = false;
    public bool doWindow1 = false;
    public bool doWindow2 = false;
    public bool doWindow6 = false;
    public bool doWindow7 = false;
    public bool doWindow8 = false;
    public bool doWindow9 = false;
    public bool doWindow10 = false;
    public bool doWindow11 = false;
    public bool doWindow12 = false;
    public bool doWindow13 = false;
    public bool doWindow14 = false;
    public int choice = -1;
    public string cname = "";
    public int nbProfiles = 1;
    public Character[] hireList;
    public List<Character> fightTeam = new List<Character>(6);
    public bool startscreen = false;
    public bool newplayer = false;
    public bool load = false;
    public bool containsIng = false;
    public bool containsTech = false;
    public bool containsGuard = false;
    public bool containsCount = false;
    public bool containsSec = false;
    public bool containsStag = false;
    private string savePath = "";
    private string trash = "";
    private Items iChoice;
    private JSONElement State;
    Character.Characterclass type;

    Character idk = null;
    float page = 0;

    public string[] Itemdesc =
        {
        "A stack of money to slap your opponent \n",
        "A stick, fairly efficient to hurt people \n" ,
        "The perfect tool for the mighty HACKERMAN \n",
        "Well, ya know... to do maths and stuff \n",
        "Used either to sweep the floor or sweep your enemy's ass \n",
        "Just staple their fingers together, it works pretty well \n",
        "A little cup of coffee to warm your heart (+PV) \n",
        "Don't underestimate the power of the donut, be careful not to have too many though (+Stats, -PV) \n",
        "Zap your enemies like a true Sith Lord \n"
    };

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

        foreach (Character chara in team)
        {

            if (chara == null)
                break;
            //Debug.Log("" + chara.c);
            switch (chara.c)
            {
                case Character.Characterclass.ING:
                    ingCount += chara.lvl;
                    break;
                case Character.Characterclass.STAG:
                    stagCount += chara.lvl;
                    break;
                case Character.Characterclass.MAN:
                    manCount += chara.lvl;
                    break;
                case Character.Characterclass.SEC:
                    secCount += chara.lvl;
                    break;
                case Character.Characterclass.COUNT:
                    comptCount += chara.lvl;
                    break;
                default:
                    break;
            }
            income -= chara.salary;
        }
        containsIng = ingCount > 0;
        /*containsStag = stagCount > 0;
        contains*/
        income += 200 + (ingCount * 2000 + stagCount * 500 + (manCount < nbEmployee / 5 ? -1000 : 0) + (nbEmployee > 10 && comptCount == 0 ? -2000 : 0)) * (1 + secCount * 1.1f);
        return income;
    }

    void updateMoney()
    {
        income = getIncome(Team);
        money += (int)income;
    }

    void Hire(int windowID)
    {
       // cname = GUILayout.TextField(cname);
        string[] classes = { "Intérim", "Stagiaire", "Secrétaire", "Ingénieur", "Manageur", "Technicien de surface", "Agent de sécurité", "Comptable" };
        choice = GUILayout.SelectionGrid(choice, classes, 2);
        type = (Character.Characterclass)choice;
        nbProfiles = (int)GUILayout.HorizontalSlider(nbProfiles, 1, 10);
        if (GUILayout.Button("Choose between " + nbProfiles + " profile. Price : " + nbProfiles * 500)/* && cname != ""*/)
        {
            if (money > nbProfiles * 500)
            {
                hireList = GenerateHireList(nbProfiles, type);
                money -= nbProfiles * 500;
                doWindow2 = true;
                choice = -1;
                doWindow0 = false;
            }
            else doWindow10 = true;
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
            doWindow0 = false;
        GUI.skin = customSkin;
    }

    void ChoicesDisplay(int WindowID)
    {
        int l = hireList.Length;
        for (int i = 0; i < (l > 3 ? 3 : l); i++)
        {
            if (i + 3 * (int)page < l)
            {
                Character candidate = hireList[i + 3 * (int)page];
                string BoxContent = candidate.cname + "\n";
                BoxContent += "Lvl : " + candidate.lvl + "\n";
                BoxContent += "Life : " + candidate.life + "\n";
                BoxContent += "Atk : " + candidate.atk + "\n";
                BoxContent += "Def : " + candidate.def + "\n";
                BoxContent += "MAtk : " + candidate.matk + "\n";
                BoxContent += "MDef : " + candidate.mdef + "\n";
                BoxContent += "Spd : " + candidate.spd + "\n";
                BoxContent += "Salary : " + candidate.salary + "\n";
                if (GUILayout.Button(BoxContent))
                {
                    Character c = new Character(type, candidate.cname, candidate.lvl);
                    c.c = type;
                    c.life = candidate.life;
                    c.atk = candidate.atk;
                    c.def = candidate.def;
                    c.matk = candidate.matk;
                    c.mdef = candidate.mdef;
                    c.spd = candidate.spd;
                    c.salary = candidate.salary;
                    Team.Add(c);
                    SaveTeam();
                    page = 0;
                    doWindow2 = !doWindow2;
                }
            }
        }
        if (l > 3)
        {
            page = GUILayout.HorizontalSlider(page, 0, (l / 3) - 1 >= 1 ? (l / 3) - 1 : 1);
            GUILayout.Box("Slide to change page");
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            page = 0;
            doWindow2 = false;
        }
        GUI.skin = customSkin;
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
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            idk = null;
            doWindow1 = false;
        }
        GUI.skin = customSkin;
    }

    void StartScreen(int windowID)
    {
        if (GUILayout.Button("New Player"))
        {
            newplayer = true;
            startscreen = false;
        }
        if (GUILayout.Button("Load Team"))
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
            int lvl = JSON.SearchJSON(T.data[i], "lvl").int_value;
            switch (JSON.SearchJSON(T.data[i], "class").string_value)
            {
                case "INTER":
                    chara = new Character(Character.Characterclass.INTER, name, lvl);
                    break;
                case "STAG":
                    chara = new Character(Character.Characterclass.STAG, name, lvl);
                    break;
                case "SEC":
                    chara = new Character(Character.Characterclass.SEC, name, lvl);
                    break;
                case "ING":
                    chara = new Character(Character.Characterclass.ING, name, lvl);
                    break;
                case "MAN":
                    chara = new Character(Character.Characterclass.MAN, name, lvl);
                    break;
                case "TECH":
                    chara = new Character(Character.Characterclass.TECH, name, lvl);
                    break;
                case "GUARD":
                    chara = new Character(Character.Characterclass.GUARD, name, lvl);
                    break;
                case "COUNT":
                    chara = new Character(Character.Characterclass.COUNT, name, lvl);
                    break;
                default:
                    chara = new Character(Character.Characterclass.PDG, name, lvl);
                    break;
            }
            chara.xp = JSON.SearchJSON(T.data[i], "xp") == null ? 0 : JSON.SearchJSON(T.data[i], "xp").int_value;
            chara.salary = JSON.SearchJSON(T.data[i], "salary").int_value;
            chara.Life = JSON.SearchJSON(T.data[i], "life").int_value;
            chara.Atk = JSON.SearchJSON(T.data[i], "atk").int_value;
            chara.Def = JSON.SearchJSON(T.data[i], "def").int_value;
            chara.Matk = JSON.SearchJSON(T.data[i], "matk").int_value;
            chara.Mdef = JSON.SearchJSON(T.data[i], "mdef").int_value;
            chara.Spd = JSON.SearchJSON(T.data[i], "spd").int_value;
            var inv = JSON.SearchJSON(T.data[i], "Inventory");
            if (inv != null)
            {
                for (int j = 0; j < inv.data.Count; j++)
                {
                    string iname = JSON.SearchJSON(inv.data[j], "name").string_value;
                    lvl = JSON.SearchJSON(inv.data[j], "lvl").int_value;
                    switch (JSON.SearchJSON(inv.data[j], "class").string_value)
                    {
                        case "MONEY":
                            chara.inventory.Add(new Money(iname, lvl));
                            break;
                        case "TASER":
                            chara.inventory.Add(new Taser(iname, lvl));
                            break;
                        case "STICK":
                            chara.inventory.Add(new Stick(iname, lvl));
                            break;
                        case "STAPLER":
                            chara.inventory.Add(new Stapler(iname, lvl));
                            break;
                        case "DONUT":
                            chara.inventory.Add(new Donut(iname, lvl));
                            break;
                        case "COMPUTER":
                            chara.inventory.Add(new Computer(iname, lvl));
                            break;
                        case "CALCULATOR":
                            chara.inventory.Add(new Calc(iname, lvl));
                            break;
                        case "COFFEE":
                            chara.inventory.Add(new Coffee(iname, lvl));
                            break;
                        case "BROOM":
                            chara.inventory.Add(new Broom(iname, lvl));
                            break;
                        default:
                            throw new System.Exception("How about no?");
                    }
                }
            }
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

            //#####################################################
              savePath = cname;
            //File.AppendAllText(".\\SavePath", cname);
            //#####################################################

            InvokeRepeating("updateMoney", 0f, 7f);
        }
    }

    void Load(int windowID)
    {
        trash = GUILayout.TextField(trash);
        string path = ".\\Assets\\Scripts\\Sauvegarde\\" + trash;

        //#####################################################
        savePath = trash;
        //File.AppendAllText(".\\SavePath", trash);
        //#####################################################

        if (GUILayout.Button("Load Team") && trash != "")
        {
            if (File.Exists(path))
            {
                JSONElement T = JSON.ParseJSONFile(path);
                // File.WriteAllText(path + "0", JSON.PrintJSON(T));
                if (T != null)
                {
                    ParseTeam(JSON.SearchJSON(T, "Team"));
                    money = JSON.SearchJSON(T, "Money").int_value;
                    baseLevel = JSON.SearchJSON(T, "Base Level").int_value;
                }
                JSONElement Inv = JSON.SearchJSON(T, "Team Inventory");
                if (Inv.Type == JSONElement.JSONType.DIC)
                {
                    for (int i = 0; i < Inv.data.Count; i++)
                    {
                        string iname = JSON.SearchJSON(Inv.data[i], "name").string_value;
                        int lvl = JSON.SearchJSON(Inv.data[i], "lvl").int_value;
                        switch (JSON.SearchJSON(Inv.data[i], "class").string_value)
                        {
                            case "MONEY":
                                TeamInventory.Add(new Money(iname, lvl));
                                break;
                            case "TASER":
                                TeamInventory.Add(new Taser(iname, lvl));
                                break;
                            case "STICK":
                                TeamInventory.Add(new Stick(iname, lvl));
                                break;
                            case "STAPLER":
                                TeamInventory.Add(new Stapler(iname, lvl));
                                break;
                            case "DONUT":
                                TeamInventory.Add(new Donut(iname, lvl));
                                break;
                            case "COMPUTER":
                                TeamInventory.Add(new Computer(iname, lvl));
                                break;
                            case "CALCULATOR":
                                TeamInventory.Add(new Calc(iname, lvl));
                                break;
                            case "COFFEE":
                                TeamInventory.Add(new Coffee(iname, lvl));
                                break;
                            case "BROOM":
                                TeamInventory.Add(new Broom(iname, lvl));
                                break;
                            default:
                                throw new System.Exception("How about no?");
                        }
                    }
                }

                InvokeRepeating("updateMoney", 0f, 7f);
                /*foreach (Character c in Team)
                    c.updateStats();*/
                load = false;
            }
        }
    }

    void WeaponShop(int windowID)
    {
        for (int i = 0; i < 9; i++)
        {
            string BoxContent = "Buy " + (i == 0 ? "STACK OF " : "") + (Items.ItemClass)(i % 9) + "\n" + Itemdesc[i % 9] + "price : 2000";
            if (GUILayout.Button(BoxContent))
            {
                if (money < 2000)
                {
                    doWindow10 = true;
                }
                else
                {
                    TeamInventory.Add(new Items(i % 9 >= 6 ? Items.ItemType.SUPPORT : Items.ItemType.WEAPON, (Items.ItemClass)(i % 9)));
                    money -= 2000;
                }
                doWindow6 = false;
            }
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            idk = null;
            doWindow6 = false;
        }
        GUI.skin = customSkin;

    }

    void ItemEquip(int windowID)
    {
        foreach (Items i in TeamInventory)
        {
            if (GUILayout.Button("" + i.c))
            {
                iChoice = i;
                doWindow8 = true;
                doWindow7 = false;
            }
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            idk = null;
            doWindow7 = false;
        }
        GUI.skin = customSkin;
    }

    void CharaChoice(int windowID)
    {
        switch (iChoice.c)
        {
            case Items.ItemClass.BROOM:
                if (!containsTech && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.TECH || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            if (c.lvl >= iChoice.lvl)
                            {
                                c.inventory.Add(iChoice);
                                TeamInventory.Remove(iChoice);
                                doWindow8 = false;
                            }
                            else GUILayout.Box("This character's level is too low");
                        }
                    }
                }
                break;
            case Items.ItemClass.CALCULATOR:
                if (!containsCount && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.COUNT || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            c.inventory.Add(iChoice);
                            TeamInventory.Remove(iChoice);
                            doWindow8 = false;
                        }
                    }
                }
                break;
            case Items.ItemClass.COFFEE:
                if (!containsSec && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.SEC || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            c.inventory.Add(iChoice);
                            TeamInventory.Remove(iChoice);
                            doWindow8 = false;
                        }
                    }
                }
                break;
            case Items.ItemClass.COMPUTER:
                if (!containsIng && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.ING || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            c.inventory.Add(iChoice);
                            TeamInventory.Remove(iChoice);
                            doWindow8 = false;
                        }
                    }
                }
                break;
            case Items.ItemClass.DONUT:
                foreach (Character c in Team)
                {
                    if ((c.c == Character.Characterclass.TECH || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                    {
                        c.inventory.Add(iChoice);
                        TeamInventory.Remove(iChoice);
                        doWindow8 = false;
                    }
                }
                break;
            case Items.ItemClass.MONEY:
                foreach (Character c in Team)
                {
                    if ((c.c == Character.Characterclass.MAN || c.c == Character.Characterclass.PDG) && GUILayout.Button(c.cname))
                    {
                        c.inventory.Add(iChoice);
                        TeamInventory.Remove(iChoice);
                        doWindow8 = false;
                    }
                }
                break;
            case Items.ItemClass.STAPLER:
                if (!containsSec && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.SEC || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            c.inventory.Add(iChoice);
                            TeamInventory.Remove(iChoice);
                            doWindow8 = false;
                        }
                    }
                }
                break;
            case Items.ItemClass.STICK:
                if (!containsGuard && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.GUARD || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            c.inventory.Add(iChoice);
                            TeamInventory.Remove(iChoice);
                            doWindow8 = false;
                        }
                    }
                }
                break;
            case Items.ItemClass.TASER:
                if (!containsGuard && !containsStag)
                {
                    GUILayout.Box("You don't have the required class to equip this item");
                    GUI.skin = closeSkin;
                    if (GUILayout.Button("Close"))
                        doWindow8 = false;
                    GUI.skin = customSkin;
                }
                else
                {
                    foreach (Character c in Team)
                    {
                        if ((c.c == Character.Characterclass.GUARD || c.c == Character.Characterclass.STAG) && GUILayout.Button(c.cname))
                        {
                            c.inventory.Add(iChoice);
                            TeamInventory.Remove(iChoice);
                            doWindow8 = false;
                        }
                    }
                }
                break;
            default:
                GUILayout.Box("You don't have the required class to equip this item");
                GUI.skin = closeSkin;
                if (GUILayout.Button("Close"))
                    doWindow8 = false;
                GUI.skin = customSkin;
                break;

        }
    }

    void ItemUpgrade(int windowID)
    {
        foreach (Items i in TeamInventory)
        {
            if (i.lvl < 3 && GUILayout.Button("" + i.c + "\n Cost : " + 5000 * (i.lvl + 1)))
            {
                if (money >= 5000)
                {
                    UpgradeWaitingList.Add(i);
                    Invoke("UpgradeItem", 120);
                    TeamInventory.Remove(i);
                    money -= 5000 * (i.lvl + 1);
                    break;
                }
                else doWindow10 = true;
                doWindow9 = false;
            }
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
            doWindow9 = false;
        GUI.skin = customSkin;
    }

    void CharaUpgrade(int windowID)
    {
        int l = Team.Count;
        for (int i = 0; i < (l > 6 ? 6 : l); i++)
        {
            if (i + 6 * (int)page < l)
            {
                Character c = Team[i + 6 * (int)page];
                if (c.lvl <= 3 && GUILayout.Button("" + c.cname + "\n Cost : " + 50000 * Mathf.Pow(3, c.lvl - 1)))
                {
                    if (money >= 50000)
                    {
                        CharaWaitingList.Add(c);
                        Invoke("UpgradeChara", 300);
                        Team.Remove(c);
                        money -= 50000 * (int)Mathf.Pow(3, c.lvl - 1);
                        break;
                    }
                    else doWindow10 = true;
                    doWindow13 = false;
                }
            }
        }
        if (l > 6)
        {
            page = GUILayout.HorizontalSlider(page, 0, (l / 6) - 1 >= 1 ? (l / 6) - 1 : 1);
            //GUILayout.Box("Slide to change page");
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            page = 0;
            doWindow13 = false;
        }
        GUI.skin = customSkin;
    }

    void MoneyError(int windowID)
    {
        GUILayout.Box("You don't have enough money to perform that action");
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
            doWindow10 = false;
        GUI.skin = customSkin;
    }

    void DisplayTeam(int windowID)
    {

        GUILayout.Box("Click on a character to see their inventory");
        int l = Team.Count;
        for (int i = 0; i < (l > 3 ? 3 : l); i++)
        {
            if (i + 3 * (int)page < l)
            {
                Character c = Team[i + 3 * (int)page];
                float xp = (float)c.xp / (float)c.xpsteps[c.lvl - 1] <= 1 ? (float)c.xp / (float)c.xpsteps[c.lvl - 1] : 1;               
                string BoxContent = "" + c.cname + "\n";
                BoxContent += "Lvl : " + (c.lvl) + "\n";
                BoxContent += "Life : " + (int)(c.Life * xp * c.step[0] * 0.5) + "\n";
                BoxContent += "Atk : " + (int)(c.Atk * xp * c.step[1] * 0.5) + "\n";
                BoxContent += "Def : " + (int)(c.def * xp * c.step[2] * 0.5) + "\n";
                BoxContent += "MAtk : " + (int)(c.matk * xp * c.step[3] * 0.5) + "\n";
                BoxContent += "MDef : " + (int)(c.mdef * xp * c.step[4] * 0.5) + "\n";
                BoxContent += "Spd : " + (int)(c.spd * xp * c.step[5] * 0.5) + "\n";
                BoxContent += "Salary : " + c.salary + "\n";                
                BoxContent += "XP : " + ((int)(xp * 100) + "%");
                if (GUILayout.Button(BoxContent))
                {
                    idk = c;
                    page = 0;
                    doWindow12 = true;
                    doWindow11 = false;
                }
            }
        }
        if (l > 3)
        {
            page = GUILayout.HorizontalSlider(page, 0, (l / 3) - 1);
            //GUILayout.Box("Slide to change page");
        }
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            page = 0;
            doWindow11 = false;
        }
        GUI.skin = customSkin;
    }

    void DisplayItems(int windowID)
    {
        GUILayout.Box("Click on an item to " + (idk == null ? "equip it." : "unequip it."));
        foreach (Items i in idk != null ? idk.inventory : TeamInventory)
        {
            if (GUILayout.Button("" + i.c + "\n LvL : " + i.lvl))
            {
                if (idk == null)
                {
                    iChoice = i;
                    doWindow8 = true;
                    doWindow12 = false;
                }
                else
                {
                    TeamInventory.Add(i);
                    idk.inventory.Remove(i);
                    doWindow12 = false;
                }
            }
        }
        if ((idk != null ? idk.inventory : TeamInventory).Count == 0)
            GUILayout.Box("Much empty, very wow");
        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            idk = null;
            doWindow12 = false;
        }
        GUI.skin = customSkin;
    }

    void ChooseTeam(int windowID)
    {
        GUILayout.Box("Choose up to 6 characters to send into battle.");
        int l = Team.Count;
        int t = fightTeam.Count;
        int i = 0;
        while (i < (l > 3 ? 3 : l))
        {
            if (i + 3 * (int)page < l)
            {
                Character c = Team[i + 3 * (int)page];
                string BoxContent = "" + c.cname + "\n";
                BoxContent += "Lvl : " + (c.lvl) + "\n";
                if (GUILayout.Button(BoxContent))
                {
                    if (t < 6)
                    {
                        fightTeam.Add(c);
                        Team.Remove(c);
                        l--;
                    }
                }
            }
            i++;
        }
        GUILayout.Box("Team :");
        int j = 0;
        string[] Content = { "Empty \n --", "Empty \n --", "Empty \n --", "Empty \n --", "Empty \n --", "Empty \n --" };

        int selected = -1;
        while (j < t)
        {
            Character c = fightTeam[j];                      
            Content[j] = "" + c.cname + "\n" + "Lvl : " + (c.lvl + 1) + "\n";
           /* if (GUILayout.Button(BoxContent))
            {
                fightTeam.Remove(c);
                t--;
                Team.Add(c);
            }*/
            j++;
        }
        selected = GUILayout.SelectionGrid(selected, Content, 2);
        if (selected != -1)
        {
            Character c = fightTeam[selected];
            fightTeam.Remove(c);
            t--;
            Team.Add(c);
            selected = -1;
        }
        if (l > 3)
        {
            page = GUILayout.HorizontalSlider(page, 0, (l / 3) - 1);
            GUILayout.Box("Slide to change page");
        }
        if (t > 0)
        {
            if (GUILayout.Button("To battle!"))
            {
                File.AppendAllText(".\\Assets\\Scripts\\Sauvegarde\\Fight\\SavePath", savePath);
                File.AppendAllText(".\\Assets\\Scripts\\Sauvegarde\\Fight\\Money", "" + money);
                GenerateEnemyTeam();
                while (fightTeam.Count > 0)
                {
                    Team.Add(fightTeam[fightTeam.Count - 1]);
                    fightTeam.RemoveAt(fightTeam.Count - 1);
                }
                //do something to get to Lea's part
                page = 0;
                doWindow14 = false;
            }
        }

        GUI.skin = closeSkin;
        if (GUILayout.Button("Close"))
        {
            while (fightTeam.Count > 0)
            {
                Team.Add(fightTeam[fightTeam.Count - 1]);
                fightTeam.RemoveAt(fightTeam.Count - 1);
            }
            page = 0;
            doWindow14 = false;
        }
        GUI.skin = customSkin;
    }

    public List<string> GetFighterNames()
    {
        List<string> names = new List<string>();
        foreach (Character c in fightTeam)
        {
            names.Add(c.cname);
        }
        return names;
    }

    void UpgradeItem()
    {
        if (UpgradeWaitingList.Count > 0)
        {
            var i = UpgradeWaitingList[0];
            UpgradeWaitingList.Remove(i);
            i.Upgrade();
            TeamInventory.Add(i);
        }
    }

    void UpgradeChara()
    {
        if (CharaWaitingList.Count > 0)
        {
            var c = CharaWaitingList[0];
            CharaWaitingList.Remove(c);
            c.lvlUp();
            Team.Add(c);
        }
    }

    void OnGUI()
    {
        // GUI.backgroundColor = Color.white;
        GUI.skin = customSkin;
        Rect window = new Rect(new Vector2(Screen.width / 2 - 250, Screen.height / 2 - 250), new Vector2(500, 100));
        if (!startscreen)
        {
            GUILayout.Box("" + money);
            GUILayout.Box("Base Level : " + (baseLevel + 1));
            GUILayout.Box("Income : " + income);
            if (baseLevel < 2 && GUILayout.Button("Upgrade Base"))
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

            if (containsIng && GUILayout.Button("Buy Weapon / Item"))
                doWindow6 = true;
            if (containsIng && GUILayout.Button("Upgrade Weapon / Item"))
                doWindow9 = true;

            if (doWindow9)
                GUILayout.Window(9, window, ItemUpgrade, "Upgrade Item");
            if (doWindow10)
                GUILayout.Window(10, window, MoneyError, "Not Enough Money");
            if (doWindow6)
                GUILayout.Window(6, window, WeaponShop, "Buy Weapon / Item");
            if (doWindow11)
                GUILayout.Window(11, window, DisplayTeam, "Team");
            if (doWindow12)
                GUILayout.Window(12, window, DisplayItems, "Inventory");
            if (doWindow13)
                GUILayout.Window(13, window, CharaUpgrade, "Choose character to upgrade");
            if (doWindow14)
                GUILayout.Window(14, window, ChooseTeam, "Choose Your fighters");

            if (GUILayout.Button("Display Team"))
                doWindow11 = true;
            if (GUILayout.Button("Display Team Inventory"))
                doWindow12 = true;
            if (GUILayout.Button("Level up character"))
                doWindow13 = true;

            if (TeamInventory.Count > 0)
            {
                /* if (GUILayout.Button("Equip Item"))
                     doWindow7 = true;*/
            }
            if (GUILayout.Button("Fight!"))
                doWindow14 = true;
            if (doWindow7)
                GUILayout.Window(7, window, ItemEquip, "Equip Item");
            if (doWindow8)
                GUILayout.Window(8, window, CharaChoice, "Choose character");
            if (GUILayout.Button("Force to next month"))
            {
                updateMoney();
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
        string path = ".\\Assets\\Scripts\\Sauvegarde\\" + Team[0].cname;
        File.Delete(path);
        //File.WriteAllText("./SavePath", path);
        JSONElement TeamSpecs = new JSONElement(JSONElement.JSONType.DIC);
        foreach (Character c in Team)
        {
            JSONElement CharaSpecs = new JSONElement(JSONElement.JSONType.DIC);
            CharaSpecs.Add("name", new JSONElement(c.cname));
            CharaSpecs.Add("lvl", new JSONElement(c.lvl));
            CharaSpecs.Add("salary", new JSONElement(c.salary));
            CharaSpecs.Add("xp", new JSONElement(c.xp));
            CharaSpecs.Add("class", new JSONElement("" + c.c));
            CharaSpecs.Add("life", new JSONElement(c.Life));
            CharaSpecs.Add("atk", new JSONElement(c.Atk));
            CharaSpecs.Add("def", new JSONElement(c.Def));
            CharaSpecs.Add("matk", new JSONElement(c.Matk));
            CharaSpecs.Add("mdef", new JSONElement(c.Mdef));
            CharaSpecs.Add("spd", new JSONElement(c.Spd));
            CharaSpecs.Add("hero", new JSONElement(1));
            JSONElement Inventory = new JSONElement(JSONElement.JSONType.DIC);
            if (c.inventory.Count > 0)
            {
                foreach (Items i in c.inventory)
                {
                    if (i != null)
                    {
                        JSONElement Item = new JSONElement(JSONElement.JSONType.DIC);
                        Item.Add("type", new JSONElement("" + i.type));
                        Item.Add("class", new JSONElement("" + i.c));
                        Item.Add("name", new JSONElement("" + i.c));
                        Item.Add("lvl", new JSONElement(i.lvl));
                        Inventory.Add("" + i.c, Item);
                    }
                }
                CharaSpecs.Add("Inventory", Inventory);
            }
            TeamSpecs.Add(c.cname, CharaSpecs);
        }
        State.Add("Team", TeamSpecs);
        if (TeamInventory.Count > 0)
        {
            JSONElement TeamInv = new JSONElement(JSONElement.JSONType.DIC);
            foreach (Items i in TeamInventory)
            {
                if (i != null)
                {
                    JSONElement Item = new JSONElement(JSONElement.JSONType.DIC);
                    Item.Add("type", new JSONElement("" + i.type));
                    Item.Add("class", new JSONElement("" + i.c));
                    Item.Add("name", new JSONElement("" + i.c));
                    Item.Add("lvl", new JSONElement(i.lvl));
                    TeamInv.Add("" + i.c, Item);
                }
            }
            State.Add("Team Inventory", TeamInv);
        }
        else State.Add("Team Inventory", new JSONElement("Empty"));

        File.WriteAllText(path, JSON.PrintJSON(State));
    }

    public Character generateEnemy()
    {
        string[] namelist = File.ReadAllLines("name_list.txt");
        GameObject chara = new GameObject();
        int lvl = Random.Range(0, 500);
        Character.Characterclass c = (Character.Characterclass)(Random.Range(0, 8));
        string name = namelist[Random.Range(0, namelist.Length - 1)];
        Character foe = new Character(c, name, lvl > 400 ? (lvl > 490 ? 3 : 2) : 1);
        int ItemLvl = Random.Range(0, 99);
        switch (c)
        {
            case Character.Characterclass.COUNT:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Calc("Ti 83+", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                break;
            case Character.Characterclass.GUARD:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Stick("Da Stick", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                if (ItemLvl > 20)
                    foe.inventory.Add(new Taser("Bzttt 3000", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                break;
            case Character.Characterclass.ING:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Computer("Thinkpad P50", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                break;
            case Character.Characterclass.MAN:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Money("A Tiny Stack", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                break;
            case Character.Characterclass.PDG:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Money("1K in small cuts", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                break;
            case Character.Characterclass.SEC:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Coffee("Nespresso", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                if (ItemLvl > 20)
                    foe.inventory.Add(new Stapler("Stapler 2006", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                break;
            case Character.Characterclass.STAG:
                if (ItemLvl > 5)
                    foe.inventory.Add(new Coffee("Nespresso", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                if (ItemLvl > 40)
                {
                    Items.ItemClass i = (Items.ItemClass)(Random.Range(0, 8));
                    switch (i)
                    {
                        case Items.ItemClass.BROOM:
                            foe.inventory.Add(new Broom("Nymbus 2000", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.CALCULATOR:
                            foe.inventory.Add(new Calc("Ti 83+", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.COMPUTER:
                            foe.inventory.Add(new Computer("Thinkpad P50", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.DONUT:
                            foe.inventory.Add(new Donut("Chocolate-glazed Donut", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.MONEY:
                            foe.inventory.Add(new Money("A Tiny Stack", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.STAPLER:
                            foe.inventory.Add(new Stapler("Stapler 2006", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.STICK:
                            foe.inventory.Add(new Stick("Da Stick", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        case Items.ItemClass.TASER:
                            foe.inventory.Add(new Taser("Bzttt 3000", ItemLvl > 70 - 20 * lvl ? ItemLvl > 90 - 10 * lvl ? 2 : 1 : 0));
                            break;
                        default:
                            break;
                    }                
                } 
                break;
        }
        if (ItemLvl >= 50)
            foe.inventory.Add(new Donut("Chocolate-glazed Donut", ItemLvl > 80 ? ItemLvl > 95 ? 2 : 1 : 0));
        return foe;
    }

    public void GenerateEnemyTeam()
    {
        List<Character> enemy = new List<Character>(6);
        for (int i = 0; i < 2 * (baseLevel + 1); i++)
        {
            enemy.Add(generateEnemy());
            enemy[i].xp += Random.Range(0, baseLevel > 0 ? baseLevel > 1 ? 800000 : 40000 : 2000);
            if (baseLevel >= 1)
            {
                int up = Random.Range(0, 100) > 95 ? 2 : 1;
                for (int j = 0; j < up; j++)
                {
                    enemy[i].lvlUp();
                }
            }
        }
        JSONElement EnemySpecs = new JSONElement(JSONElement.JSONType.DIC);
        foreach (Character c in enemy)
        {
            float xp = (float)c.xp / (float)c.xpsteps[c.lvl - 1] <= 1 ? (float)c.xp / (float)c.xpsteps[c.lvl - 1] : 1;
            JSONElement CharaSpecs = new JSONElement(JSONElement.JSONType.DIC);
            CharaSpecs.Add("name", new JSONElement(c.cname));
            CharaSpecs.Add("lvl", new JSONElement(c.lvl));
            CharaSpecs.Add("salary", new JSONElement(c.salary));
            CharaSpecs.Add("xp", new JSONElement(c.xp));
            CharaSpecs.Add("class", new JSONElement("" + c.c));
            CharaSpecs.Add("life", new JSONElement((int)(c.Life *(1 + xp * c.step[0] * 0.5))));
            CharaSpecs.Add("atk", new JSONElement((int)(c.Atk * (1 + xp * c.step[1] * 0.5))));
            CharaSpecs.Add("def", new JSONElement((int)(c.Def * (1 + xp * c.step[2] * 0.5))));
            CharaSpecs.Add("matk", new JSONElement((int)(c.Matk * (1 + xp * c.step[3] * 0.5))));
            CharaSpecs.Add("mdef", new JSONElement((int)(c.Mdef * (1 + xp * c.step[4] * 0.5))));
            CharaSpecs.Add("spd", new JSONElement((int)(c.Spd * (1 + xp * c.step[5] * 0.5))));
            CharaSpecs.Add("hero", new JSONElement(0));
            JSONElement Inventory = new JSONElement(JSONElement.JSONType.DIC);
            if (c.inventory.Count > 0)
            {
                foreach (Items i in c.inventory)
                {
                    if (i != null)
                    {
                        JSONElement Item = new JSONElement(JSONElement.JSONType.DIC);
                        Item.Add("type", new JSONElement("" + i.type));
                        Item.Add("class", new JSONElement("" + i.c));
                        Item.Add("name", new JSONElement("" + i.c));
                        Item.Add("lvl", new JSONElement(i.lvl));
                        Inventory.Add("" + i.c, Item);
                    }
                }
                CharaSpecs.Add("Inventory", Inventory);
            }
            EnemySpecs.Add(c.cname, CharaSpecs);
        }
        File.WriteAllText(".\\Assets\\Scripts\\Sauvegarde\\Fight\\Foes" , JSON.PrintJSON(EnemySpecs));

        JSONElement TeamSpecs = new JSONElement(JSONElement.JSONType.DIC);
        foreach (Character c in fightTeam)
        {
            float xp = (float)c.xp / (float)c.xpsteps[c.lvl - 1] <= 1 ? (float)c.xp / (float)c.xpsteps[c.lvl - 1] : 1;
            JSONElement CharaSpecs = new JSONElement(JSONElement.JSONType.DIC);
            CharaSpecs.Add("name", new JSONElement(c.cname));
            CharaSpecs.Add("lvl", new JSONElement(c.lvl));
            CharaSpecs.Add("salary", new JSONElement(c.salary));
            CharaSpecs.Add("xp", new JSONElement(c.xp));
            CharaSpecs.Add("class", new JSONElement("" + c.c));
            CharaSpecs.Add("life", new JSONElement((int)(c.Life * (1 + xp * c.step[0] * 0.5))));
            CharaSpecs.Add("atk", new JSONElement((int)(c.Atk * (1 + xp * c.step[1] * 0.5))));
            CharaSpecs.Add("def", new JSONElement((int)(c.Def * (1 + xp * c.step[2] * 0.5))));
            CharaSpecs.Add("matk", new JSONElement((int)(c.Matk * (1 + xp * c.step[3] * 0.5))));
            CharaSpecs.Add("mdef", new JSONElement((int)(c.Mdef * (1 + xp * c.step[4] * 0.5))));
            CharaSpecs.Add("spd", new JSONElement((int)(c.Spd *(1 +  xp * c.step[5] * 0.5))));
            CharaSpecs.Add("hero", new JSONElement(1));
            JSONElement Inventory = new JSONElement(JSONElement.JSONType.DIC);
            if (c.inventory.Count > 0)
            {
                foreach (Items i in c.inventory)
                {
                    if (i != null)
                    {
                        JSONElement Item = new JSONElement(JSONElement.JSONType.DIC);
                        Item.Add("type", new JSONElement("" + i.type));
                        Item.Add("class", new JSONElement("" + i.c));
                        Item.Add("name", new JSONElement("" + i.c));
                        Item.Add("lvl", new JSONElement(i.lvl));
                        Inventory.Add("" + i.c, Item);
                    }
                }
                CharaSpecs.Add("Inventory", Inventory);
            }
            TeamSpecs.Add(c.cname, CharaSpecs);
        }
        File.WriteAllText(".\\Assets\\Scripts\\Sauvegarde\\Fight\\Heroes", JSON.PrintJSON(TeamSpecs));
    }

    Character[] GenerateHireList(int nbChara, Character.Characterclass c)
    {
        string[] namelist = File.ReadAllLines("name_list.txt");
        Character[] hireList = new Character[nbChara];
        GameObject chara = new GameObject();
        for (int i = 0; i < nbChara; i++)
        {
            int lvl = Random.Range(0, 500);
            string name = namelist[Random.Range(0, namelist.Length - 1)];
            hireList[i] = new Character(c, name, lvl > 400 ? (lvl > 490 ? 3 : 2) : 1);
        }
        return hireList;
    }

    // Use this for initialization
    void Start()
    {
        if (!File.Exists(".\\Assets\\Scripts\\Sauvegarde\\Fight\\SavePath"))
            startscreen = true;
        else
        {
            savePath = File.ReadAllText(".\\Assets\\Scripts\\Sauvegarde\\Fight\\SavePath");
            string path = ".\\Assets\\Scripts\\Sauvegarde\\" + savePath;
            if (File.Exists(path))
            {
                JSONElement T = JSON.ParseJSONFile(path);
                JSONElement Return = JSON.ParseJSONFile(".\\Assets\\Scripts\\Sauvegarde\\Fight\\Heroes");
                // File.WriteAllText(path + "0", JSON.PrintJSON(T));
                if (T != null)
                {
                    ParseTeam(JSON.SearchJSON(T, "Team"));
                    money = int.Parse(File.ReadAllText(".\\Assets\\Scripts\\Sauvegarde\\Fight\\Money"));
                    baseLevel = JSON.SearchJSON(T, "Base Level").int_value;
                }
                foreach(Character c in Team)
                {
                    JSONElement jhg = JSON.SearchJSON(Return, "c.cname");
                    if (jhg != null)
                        c.xp = JSON.SearchJSON(jhg, "xp").int_value;                        
                }
                JSONElement Inv = JSON.SearchJSON(T, "Team Inventory");
                if (Inv.Type == JSONElement.JSONType.DIC)
                {
                    for (int i = 0; i < Inv.data.Count; i++)
                    {
                        string iname = JSON.SearchJSON(Inv.data[i], "name").string_value;
                        int lvl = JSON.SearchJSON(Inv.data[i], "lvl").int_value;
                        switch (JSON.SearchJSON(Inv.data[i], "class").string_value)
                        {
                            case "MONEY":
                                TeamInventory.Add(new Money(iname, lvl));
                                break;
                            case "TASER":
                                TeamInventory.Add(new Taser(iname, lvl));
                                break;
                            case "STICK":
                                TeamInventory.Add(new Stick(iname, lvl));
                                break;
                            case "STAPLER":
                                TeamInventory.Add(new Stapler(iname, lvl));
                                break;
                            case "DONUT":
                                TeamInventory.Add(new Donut(iname, lvl));
                                break;
                            case "COMPUTER":
                                TeamInventory.Add(new Computer(iname, lvl));
                                break;
                            case "CALCULATOR":
                                TeamInventory.Add(new Calc(iname, lvl));
                                break;
                            case "COFFEE":
                                TeamInventory.Add(new Coffee(iname, lvl));
                                break;
                            case "BROOM":
                                TeamInventory.Add(new Broom(iname, lvl));
                                break;
                            default:
                                throw new System.Exception("How about no?");
                        }
                    }
                }

                InvokeRepeating("updateMoney", 0f, 7f);
               /* foreach (Character c in Team)
                    c.updateStats();*/
            }
            else startscreen = true;
            File.Delete(".\\Assets\\Scripts\\Sauvegarde\\Fight\\SavePath");
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Team.Count > 0)
            SaveTeam();
        StartOffice.enabled = baseLevel == 0;
        InterOffice.enabled = baseLevel == 1;
        EndOffice.enabled = baseLevel == 2;
        foreach (Character c in Team)
        {
            if (c.c == Character.Characterclass.TECH) containsTech = true;
            if (c.c == Character.Characterclass.STAG) containsStag = true;
            if (c.c == Character.Characterclass.SEC) containsSec = true;
            if (c.c == Character.Characterclass.ING) containsIng = true;
            if (c.c == Character.Characterclass.GUARD) containsGuard = true;
            if (c.c == Character.Characterclass.COUNT) containsCount = true;
        }

    }

}