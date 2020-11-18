using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : BaseGameMenuController
{
    [SerializeField] private Button restart;
    [SerializeField] private Button backToMenu;

    protected override void Start()
    {
        base.Start();

        play.onClick.AddListener(OnChangeMenuStatusClicked);
        restart.onClick.AddListener(levelManager.Restart);
        backToMenu.onClick.AddListener(OnGoToMainMenuClicked);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        play.onClick.RemoveListener(OnChangeMenuStatusClicked);
        restart.onClick.RemoveListener(levelManager.Restart);
        backToMenu.onClick.RemoveListener(OnGoToMainMenuClicked);
    }
    protected override void OnChangeMenuStatusClicked()
    {
        base.OnChangeMenuStatusClicked();
        Time.timeScale =(menu.activeInHierarchy ? 0 : 1);
    }
    public void OnGoToMainMenuClicked()
    {
        LevelManager.Instance.ChangeLvl((int)Scenes.MainMenu);
    }
}
