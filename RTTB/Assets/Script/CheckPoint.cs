using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool _check = false;
    CheckPoint[] tab;
    PlayerStartingPoint playerStartingPoint;

    private void Start()
    {
        tab = GameObject.FindObjectsOfType<CheckPoint>();
        playerStartingPoint = GameObject.FindObjectOfType<PlayerStartingPoint>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (CheckPoint check in tab)
            {
                check._check = false;
            }

            _check = true;
            ChangePlayerStartingPoint();
        }
    }

    void ChangePlayerStartingPoint()
    {
        playerStartingPoint.position = transform.position;
    }
}
