using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    [SerializeField] float wallRunSpeed = 10;
    bool isWallrunning = false;
    CharacterController cc;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        cc = GameObject.FindObjectOfType<CharacterController>();
        playerController = cc.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        // Get nearest Wall
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, transform.right, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, -transform.right, out hit, 2.5f, layerMask))
        {
            /*Debug.DrawRay(transform.position, hit.point, Color.black);
            Debug.DrawRay(Vector3.Cross(hit.normal, Vector3.up) + -transform.position, transform.position, Color.cyan);*/
            //Debug.Log(Vector3.Cross(hit.normal, Vector3.up));
            //if (Input.GetKey(KeyCode.C))
            //if (cc.isGrounded == false && Input.GetButton("Fire3"))
            if (cc.isGrounded == false && Input.GetButton("Jump"))
                playerController.isWallrunning = true;
            //else if (Input.GetButtonUp("Fire3"))
            else if (Input.GetButtonUp("Jump"))
                playerController.isWallrunning = false;
            /*if (isWallrunning && Input.GetButtonDown("Jump"))
                isWallrunning = false;*/

            if (playerController.isWallrunning)
            {
                Vector3 direction = new Vector3(Vector3.Cross(hit.normal, Vector3.up).x, 0, Vector3.Cross(hit.normal, Vector3.up).z);
                if (Vector3.Dot(direction, transform.forward) < 0)
                {
                    direction *= -1;
                }
                direction.y = 0;
                cc.Move(direction * Time.deltaTime * wallRunSpeed);
            }
            //Debug.Log("Did Hit : " + hit.collider.gameObject.name);
        }
        else
        {
            playerController.isWallrunning = false;
        }
    }
}
