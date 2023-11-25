using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public GameObject UIBase;
    public GameObject UIFin;
    public GameObject UILeaderBoard;
    bool canChangeScene = false;
    int sceneIndex;
    //LeaderBoard
    public Text nameText;
    public Text scoreText;
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
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
            sceneIndex = 0;
        canChangeScene = true;
        //UI
        UIBase.SetActive(false);
        UIFin.SetActive(true);
        UILeaderBoard.SetActive(true);
        nameText.text = GameObject.FindObjectOfType<SteamScript>().pseudo;
        scoreText.text = GameObject.FindObjectOfType<CanvasScript>().niceTime;
        //Music
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Victory");
    }
}
