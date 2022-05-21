using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public Material ground;
    public Material wall;
    public Shader shader;

    void Start() {
        ground = GetComponent<Renderer>().material;
    }
}
