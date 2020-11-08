using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int demage;
    [SerializeField] private float timeDelay;
    private PlayerController player;
    private DateTime lastCounter;
    private void OnTriggerEnter2D(Collider2D info)
    {
        if ((DateTime.Now - lastCounter).TotalSeconds < 0.1f)
            return;

        lastCounter = DateTime.Now;
        player = info.GetComponent<PlayerController>();
        if (player != null)
            player.ChangeHp(-demage);
    }
    private void OnTriggerExit2D(Collider2D info)
    {
        if(player == info.GetComponent<PlayerController>())
            player = null;
    }
    private void Update()
    {
        if (player != null && (DateTime.Now - lastCounter).TotalSeconds > timeDelay)
        {
            player.ChangeHp(-demage);
            lastCounter = DateTime.Now;
        }
    }
}
