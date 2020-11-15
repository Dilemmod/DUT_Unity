using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private int maxHP;
    private int currentHP;
    [SerializeField] private float SecondsToRegenerateStamina;
    [SerializeField] private int maxSP;
    private int currentSP;
    private DateTime beginCountSP;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        currentSP = maxSP;
        currentHP = maxHP;
    }
    private bool saver = false;
    private void FixedUpdate()
    {
        if (currentSP < 100)
        {
            TimerIncreasePoints();
            if (!saver)
            {
                saver = true;
                beginCountSP = DateTime.Now;
            }
        }

    }
    private void TimerIncreasePoints()
    {
        float difference = (float)(DateTime.Now - beginCountSP).TotalSeconds;
        if (difference > SecondsToRegenerateStamina)
        {
            ChangeSP(50);
            beginCountSP = DateTime.Now;
        }
        
    }
    public void ChangeHP(int value)
    {
        if(value<0)
            playerAnimator.SetTrigger("Hit");
        currentHP += value;
        if (currentHP > maxHP)
            currentHP = maxHP;
        else if (currentHP <= 0)
            playerAnimator.SetTrigger("Died");
        Debug.Log("HP = " + currentHP);

    }
    public bool ChangeSP(int value)
    {
        //Debug.Log("Stamina Value = " + value);
        if (value < 0 && currentSP < Mathf.Abs(value))
            return false;
        currentSP += value;
        if (currentSP > maxSP)
            currentSP = maxSP;
        Debug.Log("currentSP = " + currentSP);
        return true;
    }
}
