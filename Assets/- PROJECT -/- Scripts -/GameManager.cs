using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TextMeshProUGUI pointsAmountText;
    [SerializeField] private int startPoints;

    private int _currentPoints;

    private void Awake()
    {
        _currentPoints = startPoints;
        UpdateUI();
    }

    public void AddPoints(int points)
    {
        _currentPoints += points;
        UpdateUI();
    }

    private void UpdateUI()
    {
        pointsAmountText.text= _currentPoints.ToString();
    }
}