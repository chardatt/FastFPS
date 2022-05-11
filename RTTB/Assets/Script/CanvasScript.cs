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
    // Start is called before the first frame update
    void Start()
    {
        restart = GameObject.FindObjectOfType<Restart>();
        characterController = GameObject.FindObjectOfType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = ((int)restart.timer).ToString();
        speedText.text = ((int)characterController.velocity.magnitude).ToString();
    }
}
