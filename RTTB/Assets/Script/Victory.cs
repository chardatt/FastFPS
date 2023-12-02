using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public LeaderBoard leaderBoard;
    public SteamScript steamScript;
    public Text timer;
    public GameObject UIBase;
    public GameObject UIFin;
    bool canChangeScene = false;
    int sceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        leaderBoard.GetLeaderBoard();
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
        string s = timer.text.Remove(2,1);
        s = s.Remove(4,1);
        leaderBoard.SetLeaderBoardEntry("Henry",ConvertStringToInt(s),timer.text);
        //Music
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Victory");
    }
    int ConvertStringToInt(string input)
    {
        int result;
        if (int.TryParse(input, out result))
        {
            return result;
        }
        return 0;
    }
}
