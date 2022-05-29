using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public GameObject UIBase;
    public GameObject UIFin;
    bool canChangeScene = false;
    int sceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R)) && canChangeScene)
        {
            SceneManager.LoadScene(sceneIndex);
            Time.timeScale = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneIndex++;
        canChangeScene = true;
        UIBase.SetActive(false);
        UIFin.SetActive(true);
        GameObject.FindObjectOfType<BeatController>().StopBeat();
    }
}
