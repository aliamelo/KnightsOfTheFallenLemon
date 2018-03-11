using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition(value); }
    }

    protected State _currentState; //Valeur de l'état actuel
    protected bool _inTransition;

    public virtual T GetState<T>() where T : State
    {
        T target = GetComponent<T>();
        if(target == null)
        {
            target = gameObject.AddComponent<T>();
        }
        return target;
    }

    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    public virtual void Transition(State value)
    {
        if(_currentState == value || _inTransition)
            return; //Sort car rien à faire

        _inTransition = true; //Début de la transition
        //Ne peut pas changer l'état pendant une transition (en gros ça évite les bugs)

        if(_currentState != null)
            _currentState.Exit();

        _currentState = value;

        if (_currentState != null)
            _currentState.Enter();

        _inTransition = false; //Fin de la transition
    }
}
