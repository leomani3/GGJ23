using Deckbuilder;
using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NatureObject : MonoBehaviour
{
    [SerializeField] NatureState currentState;
    [SerializeField] float punchScalePower;
    [SerializeField] private GameObject collectiblePrefab;

    [SerializeField] private float collectibleSpawnProbability;

    private List<Renderer> _renderers;
    private Animator _animator;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>().ToList();
        _animator = GetComponent<Animator>();

        SetState(NatureState.Dead);
    }

    public void SetState(NatureState newState)
    {
        if (newState == currentState) return;

        switch (newState)
        {
            case NatureState.Dead:
                SetSaturation(-1);
                CustomClock.onTick -= OnTick;
                break;
            case NatureState.Alive:
                if (_animator != null)
                {
                    _animator.SetTrigger("Idle");
                }
                SetSaturation(0);
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

    private void OnDestroy()
    {
        CustomClock.onTick -= OnTick;
    }

    private void OnDisable()
    {
        CustomClock.onTick -= OnTick;
    }

    public void OnTick()
    {
        transform.DOPunchScale(new Vector3(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f)) * punchScalePower / 2, 0.25f, 1, 1);
        if (Random.value < collectibleSpawnProbability)
        {
            SpawnCollectible();
        }
    }

    public void SpawnCollectible()
    {
        Instantiate(collectiblePrefab, transform.position, Quaternion.identity);
    }

    public void DestroyObject()
    {
        FloatingTextManager.Instance.SpawnText(transform.position, 50, FloatingTextType.Damage);
        GameManager.Instance.AddPoints(-50);
        Destroy(gameObject);
    }

    public void SetSaturation(float value)
    {
        foreach (Renderer renderer in _renderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.DOFloat(value, "_HSV_S", 0.25f);
            }
        }
    }
}

public enum NatureState
{
    None,
    Dead,
    Alive
}