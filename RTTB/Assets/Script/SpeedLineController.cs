using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpeedLineController : MonoBehaviour
{
    [SerializeField]
    CharacterController playerController;
    [SerializeField]
    VisualEffect visualEffect;
    // Start is called before the first frame update
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        //playerController = GameObject.FindObjectOfType<CharacterController>().GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        visualEffect.SetFloat("SpawnRate", 150 * playerController.velocity.magnitude / 30);
//        Debug.Log(playerController.velocity.magnitude);
    }
}
