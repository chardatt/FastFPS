using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public List<GameObject> plate = new List<GameObject>();
    public int beginIndex = 0;
    public int endIndex = 5;
    BeatController beatController;

    public bool enteringArea;
    // Start is called before the first frame update
    void Start()
    {
        beatController = GameObject.FindObjectOfType<BeatController>();
        Transform[] trans;
        trans = GetComponentsInChildren<Transform>();

        foreach (Transform go in trans)
        {
            plate.Add(go.gameObject);
        }

        plate.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enteringArea)
        {
            if (beatController.timer >= 0.85714285714)
            {
                //// PULSE A FAIRE
                Debug.Log("Test");
                PulseSlider();
            }
        }
    }

    void PulseSlider()
    {
        for (int index = 0; index < plate.Count; index++)
        {
            if ((beginIndex < endIndex && index >= beginIndex && index <= endIndex))
            {
                //Debug.Log("Premier if");
                plate[index].SetActive(true);
            }
            else if (beginIndex > endIndex && (index >= beginIndex && index <= plate.Count - 1) || beginIndex > endIndex && (index >= 0 && index <= endIndex))
            {
                //Debug.Log("Second if");
                plate[index].SetActive(true);
            }
            else
            {
                plate[index].SetActive(false);
            }
        }
        if (beginIndex < plate.Count - 1)
            beginIndex += 3;
        else
            beginIndex = 0;
        if (endIndex < plate.Count - 1)
            endIndex += 3;
        else
            endIndex = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        //// TRIGGER ZONE A PLACER
        enteringArea = true;
    }
}
