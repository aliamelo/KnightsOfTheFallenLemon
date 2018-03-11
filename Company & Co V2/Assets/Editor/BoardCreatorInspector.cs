using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]

public class BoardCreatorInspector : Editor
{
    public BoardCreator current //Getter pour avoir la Map actuelle
    {
        get
        {
            return (BoardCreator)target; //Propriété d'Unity
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Pour créer des boutons utilisant les fonctions de BoardCreator
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); //Pour inclure l'implémentation par défaut

        if (GUILayout.Button("Clear"))
            current.Clear();

        if (GUILayout.Button("Grow"))
            current.Grow();

        if (GUILayout.Button("Shrink"))
            current.Shrink();

        if (GUILayout.Button("Grow Area"))
            current.GrowArea();

        if (GUILayout.Button("Shrink Area"))
            current.ShrinkArea();

        if (GUILayout.Button("Save"))
            current.Save();

        if (GUILayout.Button("Load"))
            current.Load();

        if (GUI.changed)
            current.UpdateMarker();
    }
}
