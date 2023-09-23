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
        if (restart && finalText && timerText && speedText)
        {
            finalText.text = timerText.text;

            float minutes = Mathf.FloorToInt(restart.timer / 60);
            float seconds = Mathf.FloorToInt(restart.timer - minutes * 60);
            float milisec = (int)(restart.timer * 100 % 100);
            string niceTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milisec);
            timerText.text = niceTime;
            
            speedText.text = ((int)characterController.velocity.magnitude).ToString() + " km/h";
        }
    }
}
