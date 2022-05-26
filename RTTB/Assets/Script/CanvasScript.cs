using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    CharacterController characterController;
    public Text speedText;
    public Text timerText;
    Restart restart;
    public Text finalText;
    // Start is called before the first frame update
    void Start()
    {
        restart = GameObject.FindObjectOfType<Restart>();
        characterController = GameObject.FindObjectOfType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        finalText.text = timerText.text;
        int hours = (int)restart.timer / 3600;
        int minutes = (int)restart.timer / 60;
        int seconds = (int)restart.timer % 60;
        if (hours != 0 && minutes != 0 && seconds != 0)
            timerText.text = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();
        else if (seconds == 0 && minutes == 0 && hours == 0)
        {
            timerText.text = "00:00:00";
        }
        else if (minutes == 0 && hours == 0)
        {
            if (seconds < 10)
                timerText.text = "00:00:0" + seconds.ToString();
            else
                timerText.text = "00:00:" + seconds.ToString();
        }
        else if (hours == 0)
        {
            if (minutes < 10)
                timerText.text = "00:0" + minutes.ToString();
            else
                timerText.text = "00:" + minutes.ToString();
            if (seconds < 10)
                timerText.text += ":0" + seconds.ToString();
            else
                timerText.text += ":" + seconds.ToString();
        }
        speedText.text = ((int)characterController.velocity.magnitude).ToString() + " km/h";
    }
}
