using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomClock : MonoBehaviour
{
    public static Action onTick;
    [SerializeField] private float clockDelay;

    private float timeCpt;
    public bool _active;

    private void Update()
    {
        if (!_active) return;


        timeCpt += Time.deltaTime;
        if (timeCpt > clockDelay)
        {
            onTick?.Invoke();
            timeCpt = 0;
        }
    }
}