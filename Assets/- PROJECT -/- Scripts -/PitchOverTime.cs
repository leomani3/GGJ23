using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchOverTime : MonoBehaviour
{
    [SerializeField] private float startPitch;
    [SerializeField] private float endPitch;
    [SerializeField] private float pitchSpeed;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource= GetComponent<AudioSource>();
        _audioSource.pitch = startPitch;
    }

    private void Update()
    {
        _audioSource.pitch += pitchSpeed * Time.deltaTime;
    }
}