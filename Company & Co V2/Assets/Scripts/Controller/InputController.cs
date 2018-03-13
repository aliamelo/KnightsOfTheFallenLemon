using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent; //Pour utiliser les boutons Fire de Unity

    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    // Use this for initialization
    void Update ()
    {
        //Check des mouvements
        int x = _hor.Update();
        int y = _ver.Update();

        if(x != 0 || y != 0)
        {
            if(moveEvent != null)
            {
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
            }
        }

        //Pour les bouttons Fire
        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                {
                    fireEvent(this, new InfoEventArgs<int>(i));
                }
            }
        }
    }
    
}

class Repeater //Pas un MonoBehaviour => pas appelé par Unity, doit être appelé à la main
{
    const float threshold = 0.5f; //Temps de pause entre le moment ou le bouton est pressé et la répétition de l'action
    const float rate = 0.25f; //Vitesse de répétition de l'action
    float _next; //Cible au niveau du temps
    bool _hold; //Si le joueur continu d'appuyer sur le bouton
    string _axis; //Sauvegarder l'axe 

    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    public int Update()
    {
        int retValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));
        if (value != 0) //"Demande" si le joueur appuie sur une touche
        {
            if (Time.time > _next) //Vérifie si assez de temps est passé
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }
        return retValue; //Soit -1, 0 ou 1
    }
}

