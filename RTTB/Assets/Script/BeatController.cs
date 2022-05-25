using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BeatController : MonoBehaviour
{
    public PlatformMovement environnementObject;
    public bool notPlaying = false;
    public FMOD.Studio.EventInstance event_fmod;
    public double timer;
    public Volume postProcessVolume;
    public Bloom bloom;
    float velocity = 0;
    [SerializeField] float bloomSmoothTime;
    void Start()
    {
        event_fmod = FMODUnity.RuntimeManager.CreateInstance("event:/BeatAlexis 2");
        postProcessVolume.profile.TryGet<Bloom>(out bloom);
    }

    void Update()
    {
        bloom.intensity.value = Mathf.SmoothDamp(bloom.intensity.value, 0.5f, ref velocity, bloomSmoothTime);
        if (environnementObject.moving == true && notPlaying == true)
        {
            event_fmod.start();
            notPlaying = false;
        }
        if (!notPlaying)
        {
            if (timer >= 0.85714285714)
            {
                timer = 0;
                bloom.intensity.value = 20;
            }
            timer += Time.deltaTime;
        }
    }

    public void StopBeat()
    {
        event_fmod.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StartBeat()
    {
        event_fmod.start();
    }
}
