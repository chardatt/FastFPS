using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BeatController : MonoBehaviour
{
    public PlatformMovement environnementObject;
    public bool notPlaying = false;
    public Musics levelMusic;
    public FMOD.Studio.EventInstance event_fmod;
    public double timer;
    public Volume postProcessVolume;
    public Bloom bloom;
    float velocity = 0;
    [SerializeField] float bloomSmoothTime;
    public double tempo = 0.85714285714;

    private const double RTTBTempo = 0.42857142857;
    private const double SpeedUpTempo = 0.33898305084;
    private const double MetalTempo = 0.4;
    

    public enum Musics
    {
        RTTB,
        SpeedUp,
        Metal
    }
    
    void Awake()
    {
        string levelMusicName = $"event:/M_{levelMusic}";
        if (levelMusic == Musics.SpeedUp)
        {
            tempo = SpeedUpTempo * 2;
        }
        else if (levelMusic == Musics.Metal)
        {
            tempo = MetalTempo * 2;
        }
        else
        {
            tempo = RTTBTempo * 2;
        }
        
        event_fmod = FMODUnity.RuntimeManager.CreateInstance(levelMusicName);
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
            if (timer >= tempo)
            {
                timer = 0;
                bloom.intensity.value = 20;
            }
            timer += Time.deltaTime;
        }
    }

    public void LowPass()
    {
        event_fmod.setParameterByName("Death", 2);
    }

    public void StopBeat()
    {
        
        event_fmod.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PauseBeat(bool state)
    {
        event_fmod.setPaused(state);
    }

    public void StartBeat()
    {
        event_fmod.start();
    }
}
