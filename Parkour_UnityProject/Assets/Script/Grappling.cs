using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [SerializeField] int grapRange = 10000;
    [SerializeField] float grapSpeed = 5;
    [SerializeField] float maxSpeed = 20;
    Camera cam;
    Transform target;
    Vector3 direction;
    float inertia;
    bool firstTime = false;
    CharacterController cc;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cc = GetComponentInParent<CharacterController>();
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, grapRange) && firstTime == false)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Grappling")
                    {
//                        Debug.Log("j");
                        firstTime = true;
                        target = hit.collider.transform;
                        direction = (target.position - transform.parent.position).normalized;
                    }
                }
                else
                {
                    firstTime = false;
                    //target = null;
                }
            }
            else
            {
                Debug.Log("Test");
            }

            if (target != null)
            {
                inertia += grapSpeed * Time.deltaTime;
            }
            playerController.GravityOff();
        }
        else
            playerController.GravityOn();

        inertia -= grapSpeed/2 * Time.deltaTime;

        inertia = Mathf.Clamp(inertia, 0, maxSpeed);
        if (firstTime == true)
            cc.Move(direction * inertia);
    }
}
