using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [SerializeField] int grapRange = 10000;
    [SerializeField] float grapSpeed = 5;
    [SerializeField] float grapDecel = 0.2f;
    [SerializeField] float maxSpeed = 20;
    [SerializeField] float platformGrapDistanceFromPlayer = 1.5f;
    Camera cam;
    Transform target;
    Vector3 direction;
    float inertia;
    bool firstTime = false;
    CharacterController cc;
    PlayerController playerController;
    PlatformMovement platformMovement;
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
        #region 
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
            /*else
            {
                Debug.Log("Test");
            }*/

            if (target != null)
            {
                inertia += grapSpeed * Time.deltaTime;
            }
            playerController.GravityOff();
        }
        else
            playerController.GravityOn();

        inertia -= grapDecel * Time.deltaTime;

        inertia = Mathf.Clamp(inertia, 0, maxSpeed);
        if (firstTime == true && direction * inertia != Vector3.zero)
        {
            cc.Move(direction * inertia);
            Debug.Log("Application de l'inertie " + direction * inertia);
        }
        else
        {
            direction = Vector3.zero;
            firstTime = false;
            Debug.Log("Test de reset " + direction + " " + firstTime + " " + inertia);
        }
        #endregion

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, grapRange))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Ground")
                    {
                        platformMovement = hit.collider.GetComponent<PlatformMovement>();
                        if (platformMovement.grappable)
                        {
                            platformMovement.gameObject.transform.position = transform.parent.position + transform.parent.forward * platformGrapDistanceFromPlayer - transform.parent.up;
                            Debug.Log("grappable");
                        }
                    }
                }
                else
                {

                }
            }
        }
    }
}
