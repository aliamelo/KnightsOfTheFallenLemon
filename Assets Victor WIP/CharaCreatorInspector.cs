using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterCreation))]

public class CharaCreatorInspector : Editor
{
    public CharacterCreation current //Getter pour avoir la Map actuelle
    {
        get
        {
            return (CharacterCreation)target; //Propriété d'Unity
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Pour créer des boutons utilisant les fonctions de BoardCreator
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); //Pour inclure l'implémentation par défaut

        if (GUILayout.Button("Create"))
            current.create();

        if (GUI.changed)
            current.UpdateMarker();
    }
}