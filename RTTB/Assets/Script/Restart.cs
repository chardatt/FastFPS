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
    public GameObject quit;
    public GameObject rest;
    [SerializeField] bool lockCursor = true;
    bool menuOpen;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        stop = GameObject.FindGameObjectWithTag("Stop");
        start = GameObject.FindGameObjectWithTag("Start");
        quit = GameObject.FindGameObjectWithTag("Quit");
        rest = GameObject.FindGameObjectWithTag("Restart");
        stop.SetActive(false);
        quit.SetActive(false);
        rest.SetActive(false);
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

        if (stop == null)
        {
            stop = GameObject.FindGameObjectWithTag("UI").transform.Find("Stop").gameObject;
            start = GameObject.FindGameObjectWithTag("Start");
            quit = GameObject.FindGameObjectWithTag("UI").transform.Find("Quit").gameObject;
            rest = GameObject.FindGameObjectWithTag("UI").transform.Find("Restart").gameObject;
            stop.SetActive(false);
            quit.SetActive(false);
            rest.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (menuOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void Ft_Restart()
    {    
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Rewind");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OpenMenu()
    {
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        quit.SetActive(true);
        stop.SetActive(true);
        start.SetActive(false);
        menuOpen = true;
        Time.timeScale = 0;
    }

    void CloseMenu()
    {
        GameObject.FindObjectOfType<BeatController>().StartBeat();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        quit.SetActive(false);
        stop.SetActive(false);
        start.SetActive(true);
        menuOpen = false;
        Time.timeScale = 1;
    }

    public void OpenRestartMenu()
    {
        rest.SetActive(true);
        start.SetActive(false);
        stop.SetActive(true);
        Time.timeScale = 0;
    }

    public void Ft_Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
