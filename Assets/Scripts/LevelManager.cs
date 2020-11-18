using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayerPrefs.SetInt(GamePrefs.LastPlayedLvl.ToString(), SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt(GamePrefs.LvlPlayed.ToString() + SceneManager.GetActiveScene().buildIndex, 1);
        }
    }
    #region Singleton
    public static LevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    public void Restart()
    {
        ChangeLvl(SceneManager.GetActiveScene().buildIndex);
    }
    public void EndLevel()
    {
        ChangeLvl(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void ChangeLvl(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }
    public void ResetProgres()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
}
public enum GamePrefs
{
    LastPlayedLvl,
    LvlPlayed,
}
public enum Scenes
{
    MainMenu,
    One,
    Two,
}
