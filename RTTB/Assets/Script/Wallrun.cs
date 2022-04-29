using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    [SerializeField] float wallRunSpeed = 10;
    CharacterController cc;
    PlayerController playerController;

    //SD
    float timer;
    [SerializeField] float interval;

    void Start()
    {
        cc = GameObject.FindObjectOfType<CharacterController>();
        playerController = cc.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        // Get nearest Wall
        if ((Physics.Raycast(transform.position, transform.right, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, -transform.right, out hit, 2.5f, layerMask)
            || Physics.Raycast(transform.position, transform.forward, out hit, 2.5f, layerMask)) /*&& !Input.GetButton("Jump")*/)
        {
            if (hit.collider.tag == "Wallrun")
            {
                if (cc.isGrounded == false)
                {

                    transform.parent.SetParent(hit.collider.transform);
                    playerController.hitPointWall = hit.point;
                    //Debug.DrawRay(transform.position, transform.position - transform.parent.parent.position, Color.blue, 100);
                    playerController.isWallrunning = true;
                    Debug.Log("Wallrun trueing");
                }

                if (playerController.isWallrunning && !Input.GetButton("Jump"))
                {
                    playerController.GravityOff();
                    Vector3 direction = new Vector3(Vector3.Cross(hit.normal, Vector3.up).x, 0, Vector3.Cross(hit.normal, Vector3.up).z);
                    if (Vector3.Dot(direction, transform.forward) < 0)
                    {
                        direction *= -1;
                    }
                    direction.y = 0;

                    cc.Move(direction * Time.deltaTime * wallRunSpeed);

                    timer += Time.deltaTime;
                    if (timer >= interval)
                    {
                        timer = 0;
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Walk");
                    }
                }
            }
        }
        else
        {
            playerController.GravityOn();
            Debug.Log("Wallruning falsening");
            playerController.isWallrunning = false;
        }
    }
}
