using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : BaseGameMenuController
{

    [SerializeField] protected Button chooseLevel;
    [SerializeField] protected Button reset;

    [SerializeField] private GameObject levelMenu;
    [SerializeField] protected Button closeMenu;
    protected override void Start()
    {
        base.Start();
        chooseLevel.onClick.AddListener(UseLvlMenu);
        closeMenu.onClick.AddListener(UseLvlMenu);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        chooseLevel.onClick.RemoveListener(UseLvlMenu);
        closeMenu.onClick.RemoveListener(UseLvlMenu);
    }
    private void UseLvlMenu()
    {
        levelMenu.SetActive(!levelMenu.activeInHierarchy);
        ChangeMenuStatus();
    }

    protected override void Update()
    {
        base.Update();
    }
}
