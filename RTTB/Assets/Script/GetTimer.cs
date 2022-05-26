using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTimer : MonoBehaviour
{
    Text timerFinal;

    private void Awake()
    {
        timerFinal = GetComponent<Text>();
        
        if (timerFinal.text != "00:00:00")
        {
        }
    }   
    private void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    public void TimerGet(Text textToGet)
    {
        
        timerFinal.text = textToGet.text;
    }
}
