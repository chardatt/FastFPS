using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    PlayerController playerController;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("Moving", true);
        if (playerController.moving)
        {
            animator.SetBool("Moving", true);
        }
        else
            animator.SetBool("Moving", false);
    }
}
