using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnder : MonoBehaviour
{
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject menu;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private GameObject bars;
    [SerializeField] private AudioSource endMusic;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (endMenu == null || menu == null || playerRB == null || bars == null|| endMusic == null)
            LevelManager.Instance.EndLevel();
        else
        {
            endMusic.Play();
            playerRB.bodyType = RigidbodyType2D.Static;
            Destroy(bars);
            endMenu.SetActive(true);
            menu.SetActive(true);
        }
    }
}
