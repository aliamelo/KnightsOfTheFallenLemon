using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TacticsCamera : MonoBehaviour
{
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRigth()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
    
}
