using System;
using UnityEngine;

public class Cooldown
{
    private float duration;
    private float lastUsedTime;
    private bool isAvailable;

    public bool IsAvailable => (Time.time >= lastUsedTime + duration);

    public Cooldown(float time, bool allowFirstTime = false)
    {
        duration = time;
        if (allowFirstTime)
        {
            isAvailable = true;
        }
        else
        {
            lastUsedTime = Time.time;
            isAvailable = false;
        }
    }

    public void ResetClock()
    {
        lastUsedTime = Time.time;
        isAvailable = false;
    }
}
