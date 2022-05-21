using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    Restart restart;

    private void Start()
    {
        restart = GameObject.FindObjectOfType<Restart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            restart.OpenRestartMenu();
        }
    }
}
