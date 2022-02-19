using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, transform.right, out hit, 2.5f, layerMask) || Physics.Raycast(transform.position, -transform.right, out hit, 2.5f, layerMask))
        {
            Debug.DrawRay(transform.position, hit.point, Color.black);
            Debug.Log("Did Hit");
        }
    }
}
