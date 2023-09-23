using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerStartingPoint : MonoBehaviour
{
    public Vector3 position;
    private void Awake()
    {
        //position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
}
