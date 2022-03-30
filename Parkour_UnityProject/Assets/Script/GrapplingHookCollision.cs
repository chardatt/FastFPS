using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
    }
}
