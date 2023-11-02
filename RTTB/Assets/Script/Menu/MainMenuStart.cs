using System;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class MainMenuStart : MonoBehaviour
{
    public GameObject optionCanvas;
    public GameObject mainMenuCanvas;
    
    [Header("Option Obj")]
    public TextMeshProUGUI vSync;
    public TextMeshProUGUI resolution;
    public TextMeshProUGUI musicVolume;
    public TextMeshProUGUI sfxVolume;


    private int _indexOf;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OptionInAndOut()
    {
        optionCanvas.SetActive(!optionCanvas.activeSelf);
        if (optionCanvas.activeSelf)
            RefreshOption();
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeSelf);
    }

    void RefreshOption()
    {
        vSync.text = QualitySettings.vSyncCount == 1 ? "VSync : On" : "VSync : Off";
//        resolution.text = "Resolution : " + Screen.currentResolution;
    }

    public void ChangeResolution()
    {
        _indexOf = Array.IndexOf(Screen.resolutions, Screen.currentResolution);
        _indexOf = Screen.resolutions.Length == _indexOf - 1 ? 0 : _indexOf++;

        
        Screen.SetResolution(Screen.resolutions[_indexOf].width, Screen.resolutions[_indexOf].height, FullScreenMode.FullScreenWindow);
        RefreshOption();
    }

    public void ChangeVSync()
    {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount == 1 ? 0 : 1;
        RefreshOption();
    }
}
