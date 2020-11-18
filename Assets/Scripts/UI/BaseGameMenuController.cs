using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameMenuController : MonoBehaviour
{
    protected LevelManager levelManager;

    [SerializeField] protected GameObject menu;

    [Header("MainButttons")]
    [SerializeField] protected Button play;
    [SerializeField] protected Button settings;
    [SerializeField] protected Button quit;
    protected virtual void Start()
    {
        levelManager = LevelManager.Instance;
        quit.onClick.AddListener(levelManager.Quit);
    }
    protected virtual void OnDestroy()
    {
        quit.onClick.RemoveListener(levelManager.Quit);
    }
    protected virtual void ChangeMenuStatus()
    {
        menu.SetActive(!menu.activeInHierarchy);
    }
    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            ChangeMenuStatus();
    }
}
