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
    float spawnRate;
    float velocity = 0;
    // Start is called before the first frame update
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        //playerController = GameObject.FindObjectOfType<CharacterController>().GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnRate > playerController.velocity.magnitude)
            spawnRate = playerController.velocity.magnitude;
        else
            spawnRate = Mathf.SmoothDamp(spawnRate, playerController.velocity.magnitude, ref velocity, 5f);
        visualEffect.SetFloat("SpawnRate", 150 * spawnRate / 30);
        Debug.Log(playerController.velocity.magnitude/* + " " + spawnRate*/);
    }
}
