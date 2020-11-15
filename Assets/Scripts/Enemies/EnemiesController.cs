using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private int hp;
    public void TakeDamage(int damage)
    {
        hp -= damage; 
        if (hp < 0)
            onDeath();
        Debug.Log("Hp: " + hp+"\tDemage: " + damage);
    }

    private void onDeath()
    {
        Destroy(gameObject);
    }
}
