using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public List<Transform> posList = new List<Transform>();
    //public Vector3 translatorVector;
    Transform origin;
    double tempo;
    double timer;
    public bool moving = false;
    public bool grappable = false;
    //bool goingToTranslator = true;
    Vector3 velocity = Vector3.zero;
    public float smoothTime = 2;
    GameObject player;
    int i = 0;
    Quaternion target;
    private BeatController _beatController;
    
    // Start is called before the first frame update
    void Start()
    {
        _beatController = GameObject.FindObjectOfType<BeatController>();
        tempo = _beatController.tempo;
        origin = new GameObject().transform;
        origin.position = transform.position;
        origin.rotation = transform.rotation;
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/Platform", transform.position);
                moving = true;
                timer = 0;
            }

            if (moving)
            {
//                Debug.Log("Test " + transform.eulerAngles + " " + posList[i].eulerAngles);
                //target.eulerAngles = Vector3.SmoothDamp(transform.rotation.eulerAngles, posList[i].rotation.eulerAngles, ref velocity, smoothTime);
                if (transform.eulerAngles != posList[i].eulerAngles)
                    transform.eulerAngles = posList[i].eulerAngles;
                transform.position = Vector3.SmoothDamp(transform.position, posList[i].position, ref velocity, smoothTime);
                if (Vector3.Distance(posList[i].position, transform.position) < 0.05f)
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
