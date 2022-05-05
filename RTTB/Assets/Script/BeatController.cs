using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    public PlatformMovement environnementObject;
    public bool notPlaying = false;
    private FMOD.Studio.EventInstance event_fmod;
    void Start()
    {
        event_fmod = FMODUnity.RuntimeManager.CreateInstance("event:/BeatAlexis");
    }

    void Update()
    {
        if (environnementObject.moving == true && notPlaying == true)
        {
            event_fmod.start();
            notPlaying = false;
        }
    }

    public void StopBeat()
    {
        event_fmod.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
