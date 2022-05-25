using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public List<GameObject> plate = new List<GameObject>();
    public int beginIndex = 0;
    public int endIndex = 5;
    float timer;

    bool enteringArea;
    // Start is called before the first frame update
    void Start()
    {
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
            timer += Time.deltaTime;
            if (timer >= 0.85714285714)
            {
                //// PULSE A FAIRE
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //// TRIGGER ZONE A PLACER
        enteringArea = true;
    }
}
