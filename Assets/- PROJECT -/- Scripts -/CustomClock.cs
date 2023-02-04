using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomClock : MonoBehaviour
{
    public static Action onTick;
    [SerializeField] private float clockDelay;

    private float timeCpt;

    private void Update()
    {
        timeCpt += Time.deltaTime;
        if (timeCpt > clockDelay)
        {
            onTick?.Invoke();
            timeCpt = 0;
        }
    }
}