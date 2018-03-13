using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NE CHECK PAS si on doit lock une option (Si a bougé etc.)
public class CategorySelectionState : BaseAbilityMenuState
{
    protected override void LoadMenu() //Lock les options ici si besoin
    {
        if(menuOptions == null)
        {
            menuTitle = "Action";
            menuOptions = new List<string>(3);
            menuOptions.Add("Attaque");
            menuOptions.Add("Attaque Spé.");
            menuOptions.Add("Objet");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    //Action de confirmer (à voir + en détails plus tard)
    protected override void Confirm()
    {
        switch(abilityMenuPanelController.selection)
        {
            case 0:
                Attack();
                break;
            case 1:
                SetCategory(0);
                break;
            case 2:
                SetCategory(1);
                break;
        }
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    void Attack()
    {
        turn.hasUnitActed = true;
        if(turn.hasUnitMoved)
        {
            turn.lockMove = true; //Lock le mouvement car le joueur a attaqué
        }
        owner.ChangeState<CommandSelectionState>();
    }

    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}
