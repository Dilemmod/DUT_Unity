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

        play.onClick.AddListener(ChangeMenuStatus);
        restart.onClick.AddListener(levelManager.Restart);
        backToMenu.onClick.AddListener(GoToMainMenu);

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        play.onClick.RemoveListener(ChangeMenuStatus);
        restart.onClick.RemoveListener(levelManager.Restart);
        backToMenu.onClick.RemoveListener(GoToMainMenu);
    }
    protected override void ChangeMenuStatus()
    {
        base.ChangeMenuStatus();
        Time.timeScale =(menu.activeInHierarchy ? 0 : 1);
    }
    public void GoToMainMenu()
    {
        LevelManager.Instance.ChangeLvl((int)Scenes.MainMenu);
    }
}
