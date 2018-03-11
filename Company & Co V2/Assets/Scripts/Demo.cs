using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnEnable();
        OnDisable();
	}

    void OnEnable()
    {
        InputController.moveEvent += OnMoveEvent;
        InputController.fireEvent += OnFireEvent;
    }

    private void OnFireEvent(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("Fire " + e.info);
    }

    private void OnMoveEvent(object sender, InfoEventArgs<Point> e)
    {
        Debug.Log("Move " + e.info.ToString());
    }

    void OnDisable()
    {
        InputController.moveEvent -= OnMoveEvent;
        InputController.fireEvent -= OnFireEvent;
    }
}
