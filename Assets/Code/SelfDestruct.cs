using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float delay = 5.0f;

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if(Time.time > spawnTime + delay)
        {
            Destroy(gameObject);
        }
    }
}
