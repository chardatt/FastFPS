using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlissadeScript : MonoBehaviour
{
    CharacterController cc;
    PlayerController playerController;
    [SerializeField] int dashSpeed = 300;
    [SerializeField] float dashTime = 1;
    float timer = 0;
    Vector3 direction;
    bool getUp = false;
    float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cc.height = 1;
            playerController.canMove = false;
            direction = transform.forward;
            speed = dashSpeed - speed * (Time.deltaTime * 0.5f);
        }
        if (Input.GetButtonUp("Fire1") || Input.GetButtonDown("Jump") || getUp)
        {
            cc.height = 2;
            playerController.canMove = true;
            speed = 0;
            timer = 0;
            getUp = false;
        }

        if (speed != 0)
        {
            timer += Time.deltaTime;
            cc.SimpleMove(direction * speed * Time.deltaTime);
            if (timer >= dashTime)
            {
                getUp = true;
            }
        }
    }
}