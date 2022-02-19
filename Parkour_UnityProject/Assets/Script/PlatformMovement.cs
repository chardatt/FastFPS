using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Vector3 translatorVector;
    Vector3 origin;
    public float tempo;
    float timer;
    bool moving = false;
    bool goingToTranslator = true;
    Vector3 velocity = Vector3.zero;
    public float smoothTime = 2;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= tempo)
        {
            moving = true;
            timer = 0;
        }

        if (moving && goingToTranslator == true)
        {
            /*if (GetComponentInChildren<CharacterController>())
            {
                if (player = GetComponentInChildren<CharacterController>().gameObject)
                {
                    player.transform.position += Vector3.SmoothDamp(transform.position, translatorVector, ref velocity, smoothTime);
                }
            }*/
            transform.position = Vector3.SmoothDamp(transform.position, translatorVector, ref velocity, smoothTime);
            if (Vector3.Distance(translatorVector, transform.position) < 0.5f)
            {
                goingToTranslator = false;
                moving = false;
            }
        }

        if (moving && goingToTranslator == false)
        {
            /*if (GetComponentInChildren<CharacterController>())
            {
                if (player = GetComponentInChildren<CharacterController>().gameObject)
                {
                    player.transform.position += Vector3.SmoothDamp(transform.position, translatorVector, ref velocity, smoothTime);
                }
            }*/
            transform.position = Vector3.SmoothDamp(transform.position, origin, ref velocity, smoothTime);
            if (Vector3.Distance(origin, transform.position) < 0.5f)
            {
                goingToTranslator = true;
                moving = false;
            }
        }
    }
}
