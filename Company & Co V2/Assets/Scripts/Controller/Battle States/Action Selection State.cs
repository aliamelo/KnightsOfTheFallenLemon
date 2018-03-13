using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : BaseAbilityMenuState
{
    public static int category;
    string[] attaqueSpeOption = new string[] { "Insulte", "Truc intelligent", "La CHANCLA" };
    string[] objets = new string[] { "Café", "Donut", "Laptop" };

    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>(3);

        if (category == 0)
        {
            menuTitle = "Attaque Spé.";
            SetOptions(attaqueSpeOption);
        }
        else
        {
            menuTitle = "Objet";
            SetOptions(objets);
        }
        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    protected override void Confirm()
    {
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        owner.ChangeState<CommandSelectionState>();
    }

    protected override void Cancel()
    {
        owner.ChangeState<CategorySelectionState>();
    }

    void SetOptions(string[] options)
    {
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }
}
