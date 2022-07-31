using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public const float Period = 3.0f;
    public float Clock { get; set; } = 0.0f;

    public void Tick()
    {
        Clock -= Time.deltaTime;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
