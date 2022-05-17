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
    [SerializeField] bool lockCursor = true;
    bool menuOpen;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OpenMenu()
    {
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        quit.SetActive(false);
        stop.SetActive(false);
        start.SetActive(true);
        menuOpen = false;
        Time.timeScale = 1;
    }

    public void Ft_Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
