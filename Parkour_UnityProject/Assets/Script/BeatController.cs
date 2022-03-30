using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    public PlatformMovement environnementObject;
    public bool notPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        //environnementObject = GameObject.FindObjectOfType<PlatformMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(environnementObject.timer + " " + environnementObject.tempo);
        if (environnementObject.timer >= environnementObject.tempo)*/
        /*if (environnementObject.moving && notPlaying == false)
        {
            notPlaying = true;
            //Debug.Log("Test");
            
        }*/

        if (environnementObject.moving == true && notPlaying == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Beat");
            notPlaying = false;
        }
    }
}
