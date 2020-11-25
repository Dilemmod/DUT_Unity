using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : BaseGameMenuController
{
    [Header("Buttons")]
    [SerializeField] private Button restart1;
    [SerializeField] private Button backToMenu;

    [Header("GameOver")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button restart2;
    protected override void Start()
    {
        base.Start();
        play.onClick.AddListener(OnChangeMenuStatusClicked);
        restart2.onClick.AddListener(levelManager.Restart);
        restart1.onClick.AddListener(levelManager.Restart);
        backToMenu.onClick.AddListener(OnGoToMainMenuClicked);
        

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        play.onClick.RemoveListener(OnChangeMenuStatusClicked);
        restart2.onClick.AddListener(levelManager.Restart);
        restart1.onClick.RemoveListener(levelManager.Restart);
        backToMenu.onClick.RemoveListener(OnGoToMainMenuClicked);
    }
    protected override void OnChangeMenuStatusClicked()
    {
        base.OnChangeMenuStatusClicked();
        Time.timeScale =(menu.activeInHierarchy ? 0 : 1);
    }
    public void OnPlayerDeath()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnGoToMainMenuClicked()
    {
        LevelManager.Instance.ChangeLvl((int)Scenes.MainMenu);
    }
}
