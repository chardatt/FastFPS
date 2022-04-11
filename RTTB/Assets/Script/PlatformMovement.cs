using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public List<Vector3> posList = new List<Vector3>();
    //public Vector3 translatorVector;
    Vector3 origin;
    public float tempo;
    float timer;
    public bool moving = false;
    public bool grappable = false;
    bool goingToTranslator = true;
    Vector3 velocity = Vector3.zero;
    public float smoothTime = 2;
    GameObject player;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        posList.Add(origin);
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (grappable == false)
        {
            timer += Time.deltaTime;
            if (timer >= tempo)
            {
                moving = true;
                timer = 0;
            }

            if (moving)
            {
                transform.position = Vector3.SmoothDamp(transform.position, posList[i], ref velocity, smoothTime);
                if (Vector3.Distance(posList[i], transform.position) < 0.05f)
                {
                    if (i < posList.Count - 1)
                        i++;
                    else
                        i = 0;
                    moving = false;
                }
            }
        }

        /*if (moving && goingToTranslator == true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, translatorVector, ref velocity, smoothTime);
            if (Vector3.Distance(translatorVector, transform.position) < 0.5f)
            {
                goingToTranslator = false;
                moving = false;
            }
        }

        if (moving && goingToTranslator == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, origin, ref velocity, smoothTime);
            if (Vector3.Distance(origin, transform.position) < 0.5f)
            {
                goingToTranslator = true;
                moving = false;
            }
        }*/
    }
}
