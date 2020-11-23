using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    private Button button;
    [SerializeField] private Scenes scene;
    void Start()
    {
        button = GetComponent<Button>();
        button.transform.GetChild(1).GetComponent<Image>().enabled = false;
        GetComponentInChildren<Text>().text = ((int)scene).ToString();
        if (!PlayerPrefs.HasKey(GamePrefs.LvlPlayed.ToString() + ((int)scene).ToString()))
        {
            button.transform.GetChild(1).GetComponent<Image>().enabled = true;
            button.interactable = false;
            return;
        }
        button.onClick.AddListener(OnChangeLevelClicked);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void Update()
    {
        
    }
    void OnChangeLevelClicked()
    {
        LevelManager.Instance.ChangeLvl((int)scene);
    }
}
