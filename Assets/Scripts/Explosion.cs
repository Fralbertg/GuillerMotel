using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void FixedUpdate()
    {
        Destroy(gameObject, 0.6f);
    }
}
