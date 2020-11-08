using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private int maxHp;
    private int currentHp;
    void Start()
    {
        currentHp = maxHp;
    }
    public void ChangeHp(int value)
    {
        currentHp += value;
        if (currentHp > maxHp)
            currentHp = maxHp;
        else if (currentHp <= 0)
            OnDeath();
        Debug.Log("Value = "+value);
        Debug.Log("HP = " + currentHp);

    }
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
