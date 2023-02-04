using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureObject : MonoBehaviour
{
    [SerializeField] NatureState currentState;

    private void Awake()
    {
        
    }

    public void SetState(NatureState newState)
    {
        switch (currentState)
        {
            case NatureState.Dead:
                break;
            case NatureState.Alive:
                transform.DOPunchScale(new Vector3(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1)), 1f, 1, 1);
                break;
        }
    }

    public void test()
    {
        SetState(NatureState.Alive);
    }
}

public enum NatureState
{
    Dead,
    Alive
}