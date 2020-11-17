using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomController : EnemyControllerBase
{
    [SerializeField] protected float angerRange;
    private PlayerController player;
    protected override void Start()
    {
        base.Start();

        player = FindObjectOfType<PlayerController>();
        StartCoroutine(ScanForPlayer());
    }
    protected IEnumerator ScanForPlayer()
    {
        while (true)
        {
            CheckPlayerInRange();
            yield return new WaitForSeconds(0.1f);
        }

    }
    protected void CheckPlayerInRange()
    {
        if (player == null)
            return;
        if (Vector2.Distance(transform.position, player.transform.position) < angerRange)
        {
            player.TakeDamage(collisionDamage, DamageType.PowerStrike, transform);
            ChangeState(EnemyState.Death);
        }
    }
    private void EndBoom()
    {
        Destroy(gameObject);
    }
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (currentState)
        {
            case EnemyState.Idle:
                    enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Move:
                    startPoint = transform.position;
                break;
        }
    }
}
