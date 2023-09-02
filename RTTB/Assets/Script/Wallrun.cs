using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    [SerializeField] float wallRunSpeed = 10;
    CharacterController cc;
    PlayerController playerController;
    public FMOD.Studio.EventInstance event_fmod;
    //SD
    bool playing;
    [SerializeField] float interval;
    [SerializeField] CinemachineVirtualCamera vCam;

    void Start()
    {
        cc = GameObject.FindObjectOfType<CharacterController>();
        playerController = cc.gameObject.GetComponent<PlayerController>();
        event_fmod = FMODUnity.RuntimeManager.CreateInstance("event:/Wallride");
    }

    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        RaycastHit rightHit;
        RaycastHit leftHit;
        // Get nearest Wall
        if ((Physics.Raycast(transform.position, transform.right, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, -transform.right, out hit, 2.5f, layerMask)
            || Physics.Raycast(transform.position, transform.forward, out hit, 2.5f, layerMask)) && !Input.GetButton("Jump") && playerController.wallrunTimer >= playerController.wallrunCD)
        {
            if (Physics.Raycast(transform.position, -transform.right, out leftHit, 2.5f, layerMask) &&
                leftHit.transform.CompareTag("Wallrun"))
            {
                Tilt(false);
                Debug.Log("left");
            }

            if (Physics.Raycast(transform.position, transform.right, out rightHit, 2.5f, layerMask) && rightHit.transform.CompareTag("Wallrun"))
                Tilt(true);
            if (hit.collider.tag == "Wallrun" && hit.collider.gameObject != playerController.wallrunTransform)
            {
                if (cc.isGrounded == false)
                {

                    transform.parent.SetParent(hit.collider.transform);
                    playerController.hitPointWall = hit.point;
                    //Debug.DrawRay(transform.position, transform.position - transform.parent.parent.position, Color.blue, 100);
                    playerController.isWallrunning = true;
                    //Debug.Log("Wallrun trueing");
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

                    cc.Move(direction * Time.deltaTime);

                    if (playing == false)
                    {
                        event_fmod.start();
                        playing = true;
                    }
                }
            }
        }
        else if (!Input.GetButton("Jump"))
        {
            event_fmod.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            playing = false;
            playerController.GravityOn();
            //Debug.Log("Wallruning falsening");
            playerController.isWallrunning = false;
            UnTilt();
        }

        if (Input.GetButton("Jump"))
        {
                UnTilt();
        }
    }

    //private Vector3 currentVelocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float value = 20;

    void Tilt(bool right)
    {
        if (right)
            vCam.m_Lens.Dutch = Mathf.Lerp(vCam.m_Lens.Dutch, value, smoothTime);
        else
            vCam.m_Lens.Dutch = Mathf.Lerp(vCam.m_Lens.Dutch, -value, smoothTime);
    }

    void UnTilt()
    {
        vCam.m_Lens.Dutch = Mathf.Lerp(vCam.m_Lens.Dutch, 0, smoothTime);
    }
}
