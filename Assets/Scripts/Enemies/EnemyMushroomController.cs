using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomController : EnemyControllerBase
{
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
