using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{
    public void ReturnToMenu()
    {
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        SceneManager.LoadScene(0);
    }
}
