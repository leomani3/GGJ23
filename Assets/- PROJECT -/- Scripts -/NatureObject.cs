using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureObject : MonoBehaviour
{
    [SerializeField] NatureState currentState;
    [SerializeField] float punchScalePower;
    [SerializeField] private GameObject collectiblePrefab;

    [SerializeField] private float collectibleSpawnProbability;

    public void SetState(NatureState newState)
    {
        switch (newState)
        {
            case NatureState.Dead:
                CustomClock.onTick -= OnTick;
                break;
            case NatureState.Alive:
                CustomClock.onTick += OnTick;
                transform.DOPunchScale(new Vector3(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f)) * punchScalePower, 0.5f, 1, 1);
                break;
        }
        currentState = newState;
    }

    [ButtonMethod]
    public void test()
    {
        SetState(NatureState.Alive);
    }

    public void OnTick()
    {
        print("tick");
        transform.DOPunchScale(new Vector3(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f)) * punchScalePower / 2, 0.25f, 1, 1);
        if (Random.value < collectibleSpawnProbability)
        {
            SpawnCollectible();
        }
    }

    public void SpawnCollectible()
    {
        Instantiate(collectiblePrefab);
    }
}

public enum NatureState
{
    Dead,
    Alive
}