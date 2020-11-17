using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private int currentHP;
    [SerializeField] private int maxHP;
    [SerializeField] private Slider sliderHP;
    [SerializeField] private int maxSP;
    [SerializeField] private Slider sliderSP;
    [SerializeField] private float SecondsToRegenerateStamina;
    Movement_controller playerMovment;
    private int currentSP;
    private DateTime beginCountSP;

    private bool canBeDamaged=true;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovment = GetComponent<Movement_controller>();
        playerMovment.OnGetHurt += OnGetHurt;
        currentSP = maxSP;
        currentHP = maxHP;
        sliderHP.maxValue = maxHP;
        sliderHP.value = maxHP;
        sliderSP.maxValue = maxSP;
        sliderSP.value = maxSP;
        StartCoroutine(IncreaseStaminaPoints());
    }
    private bool saver = false;
    protected IEnumerator IncreaseStaminaPoints()
    {
        while (true)
        {
            ChangeSP(1);
            yield return new WaitForSeconds(SecondsToRegenerateStamina);
        }
    }
    public void TakeDamage(int damage, DamageType type = DamageType.Casual,Transform enemy=null)
    {
        if (!canBeDamaged)
            return;
        currentHP -= damage;
        if (currentHP <= 0)
            playerAnimator.SetBool("Death",true);
        switch (type)
        {
            case DamageType.PowerStrike:
                playerMovment.GetHurt(enemy.position);
                break;
        }
        sliderHP.value = currentHP;
    }
    private void OnGetHurt(bool canBeDamaged)
    {
        this.canBeDamaged = canBeDamaged;
    }
    public void RestoredHP(int value)
    {
        currentHP += value;
        if (currentHP > maxHP)
            currentHP = maxHP;
        sliderHP.value = currentHP;
    }
    public bool ChangeSP(int value)
    {
        if (value < 0 && currentSP < Mathf.Abs(value))
            return false;
        currentSP += value;
        if (currentSP > maxSP)
            currentSP = maxSP;
        sliderSP.value = currentSP;
        return true;
    }
}
