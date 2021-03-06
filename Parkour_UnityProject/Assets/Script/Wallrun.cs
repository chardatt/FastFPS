using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    [SerializeField] float wallRunSpeed = 10;
    bool isWallrunning = false;
    CharacterController cc;
    PlayerController playerController;

    //SD
    float timer;
    [SerializeField] float interval;

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
        if ((Physics.Raycast(transform.position, transform.right, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, -transform.right, out hit, 2.5f, layerMask)))
        {
            if (hit.collider.tag == "Wallrun")
            {
                if (Input.GetButtonDown("Jump") && playerController.isWallrunning)
                {
                    playerController.isWallrunning = false;
                    //Debug.Log("Wallrun falsing");
                }
                else if (cc.isGrounded == false && Input.GetButtonDown("Jump"))
                {
                    playerController.isWallrunning = true;
                    //Debug.Log("Wallrun trueing");
                }

                if (playerController.isWallrunning)
                {
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
            //Debug.Log("Did Hit : " + hit.collider.gameObject.name);
        }
        else
        {
            //Debug.Log("Wallrun falsing");
            playerController.isWallrunning = false;
        }
    }
}
