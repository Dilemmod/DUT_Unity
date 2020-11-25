using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseGameMenuController
{
    [Header("Main menu")]
    [SerializeField] protected Button chooseLevel;
    [SerializeField] protected Button reset;

    [SerializeField] private GameObject levelMenu;
    [SerializeField] protected Button closeMenu;

    private int lvl=1;
    protected override void Start()
    {
        base.Start();
        chooseLevel.onClick.AddListener(OnLvlMenuClecked);
        closeMenu.onClick.AddListener(OnLvlMenuClecked);
        play.onClick.AddListener(OnPlayClicked);
        reset.onClick.AddListener(OnResetClicked);

       
        
        if (PlayerPrefs.HasKey(GamePrefs.LastPlayedLvl.ToString()))
        {
            play.GetComponentInChildren<Text>().text = "CONTINUE";
            lvl = PlayerPrefs.GetInt(GamePrefs.LastPlayedLvl.ToString());
        }

        audioManager.Play(UIClipName.BackgroundMusic);
    }
    protected override void Update() { }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        reset.onClick.RemoveListener(levelManager.ResetProgres);
        chooseLevel.onClick.RemoveListener(OnLvlMenuClecked);
        closeMenu.onClick.RemoveListener(OnLvlMenuClecked);
        play.onClick.RemoveListener(OnPlayClicked);
    }
    private void OnLvlMenuClecked()
    {
        audioManager.Play(UIClipName.LvlMenu);
        levelMenu.SetActive(!levelMenu.activeInHierarchy);
        OnChangeMenuStatusClicked();
    }
    private void OnResetClicked()
    {
        audioManager.Play(UIClipName.Reset);
        play.GetComponentInChildren<Text>().text = "NEW GAME";
        levelManager.ResetProgres();
    }
    private void OnPlayClicked()
    {
        audioManager.Play(UIClipName.Play);
        levelManager.ChangeLvl(lvl);
    }
}
