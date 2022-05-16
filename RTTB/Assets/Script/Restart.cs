using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public float timer;
    private static Restart instance;
    public GameObject stop;
    public GameObject start;
    bool menuOpen;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ft_Restart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen)
            {
                CloseMenu();
            }
            else
                OpenMenu();
        }
    }

    public void Ft_Restart()
    {    
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OpenMenu()
    {
        stop.SetActive(true);
        start.SetActive(false);
        menuOpen = true;
        Time.timeScale = 0;
    }

    void CloseMenu()
    {
        stop.SetActive(false);
        start.SetActive(true);
        menuOpen = false;
        Time.timeScale = 1;
    }
}
