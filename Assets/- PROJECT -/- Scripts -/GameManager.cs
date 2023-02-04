using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TextMeshProUGUI pointsAmountText;
    [SerializeField] private int startPoints;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Planet planet;
    [SerializeField] private Digger digger;

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

    public void StartGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        planet.Active = true;
        digger.active = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}