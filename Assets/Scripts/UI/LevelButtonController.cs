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
        button.onClick.AddListener(ChangeLevel);
        GetComponentInChildren<Text>().text = ((int)scene).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeLevel()
    {
        LevelManager.Instance.ChangeLvl((int)scene);
    }
}
