using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour
{
    public float lifetime = 1;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
