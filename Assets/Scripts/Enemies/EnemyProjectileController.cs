using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter2D(Collider2D info)
    {
        PlayerController player = info.GetComponent<PlayerController>();
        if (player != null)
            player.TakeDamage(damage);
        else
            return;
        Destroy(gameObject, 0.05f);
    }
}
