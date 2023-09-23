using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSet : MonoBehaviour
{
    public void ChooseLevel(int index)
    {
        GameObject.FindObjectOfType<BeatController>().StopBeat();
        SceneManager.LoadScene(index);
    }
}
