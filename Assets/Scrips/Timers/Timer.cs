using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float time;

    private bool timerStart = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
            time += Time.deltaTime * 1;
        }
    }

    public void ResetTimer()
    {
        time = 0;
    }
    public void StartTimer()
    {
        timerStart = true;
    }

    public void StopTimer()
    {
        timerStart = false;
        time = 0;
    }
}
