using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuPanelController : MonoBehaviour
{
    //Le code ça va ici
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    const string EntryPoolKey = "AbilityMenuPanel.Entry";
    const int MenuCount = 4;

    [SerializeField] GameObject entryPrefab; //Prefab du menu des entrées => instancier les objets "pooled"
    [SerializeField] Text titleLabel; //Pour le header (contexte)
    [SerializeField] Panel panel; 
    [SerializeField] GameObject canvas; //Ref au canvas

    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(); //Liste contenant toutes les entrées actives du menu

    public int selection { get; private set; } //valeur représentant l'index sélectionner du menu

    private void Awake() //Awake est appelé quand le script est en train de charger => initialiser des variables AVANT le début du jeu
    {
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    private void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.SetActive(false); //Désactive le canvas
    }

    AbilityMenuEntry Dequeue()
    {
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();
        entry.transform.SetParent(panel.transform, false); //pour mettre le parent du transform, bool (WorldPositionStays) si la position ou autre du parent est modifiée telles que l'objet garde les même paramètres
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true); //Active le gameObject entry
        entry.Reset();
        return entry;
    }

    void Enqueue(AbilityMenuEntry entry)
    {
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    //Clear TOUTES les entrées
    void Clear()
    {
        for (int i = menuEntries.Count - 1; i >= 0; --i)
            Enqueue(menuEntries[i]);
        menuEntries.Clear();
    }

    Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseInOutQuad;
        return t;
    }

    bool SetSelection(int value)
    {
        if (menuEntries[value].IsLocked)
            return false;

        //Déselectionne l'entrée sélectionnée précédente
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = false;

        selection = value;

        //Sélectionne la nouvelle entrée
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = true;

        return true;
    }

    public void Next()
    {
        //Test toutes les entrées pour voir si elles sont pas bloquées
        for (int i = selection + 1; i < selection + menuEntries.Count; ++i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }
    public void Previous()
    {
        for (int i = selection - 1 + menuEntries.Count; i > selection; --i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    //Pour charger et afficher le menu
    public void Show(string title, List<string> options)
    {
        canvas.SetActive(true);
        Clear();
        titleLabel.text = title;
        for (int i = 0; i < options.Count; ++i)
        {
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            menuEntries.Add(entry);
        }
        SetSelection(0);
        TogglePos(ShowKey);
    }

    //Bloque l'action move si a déjà bougé
    public void SetLocked(int index, bool value)
    {
        if (index < 0 || index >= menuEntries.Count)
            return;

        menuEntries[index].IsLocked = value;
        if (value && selection == index)
            Next();
    }

    //Pour dégager le panel quand l'utilisateur confirme la sélection
    public void Hide()
    {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate (object sender, System.EventArgs e)
        {
            if (panel.CurrentPosition == panel[HideKey])
            {
                Clear();
                canvas.SetActive(false);
            }
        };
    }
}
