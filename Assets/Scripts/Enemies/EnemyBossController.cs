using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyBossController : EnemyArcherController
{
    [Header("Strike")]
    [SerializeField] private Transform strikePoint;
    [SerializeField] private int strikeDamage;
    [SerializeField] private float strikeRange;
    [SerializeField] private LayerMask enemies;

    [Header("PowerStrike")]
    [SerializeField] private Collider2D strikeCollider;
    [SerializeField] private int powerStrikeDamage;
    [SerializeField] private float powerStrikeRange;
    [SerializeField] private float powerStrikeSpeed;

    [Header("Tramsition")]
    [SerializeField] private float waitTime;

    private float currentStrikeRange;
    private bool fightStarted;

    private EnemyState stateOnHold;
    private EnemyState[] attackStates = { EnemyState.Strike, EnemyState.PowerStrike, EnemyState.Shoot};
    
    #region UnityMethods
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currentState == EnemyState.Move && attacking)
        {
            TurnToPlayer();
            if (CanAttack())
                ChangeState(stateOnHold);
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(strikePoint.position, new Vector3(strikeRange, strikeRange, 0));
    }
    #endregion
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);

        switch (currentState)
        {
            case EnemyState.PowerStrike:
            case EnemyState.Strike:
                attacking = true;
                currentStrikeRange = state == EnemyState.Strike ? strikeRange : powerStrikeRange;
                enemyRb.velocity = Vector2.zero;
                if (!CanAttack())
                {
                    stateOnHold = state;
                    enemyAnimator.SetBool(currentState.ToString(), false);
                    ChangeState(EnemyState.Move);
                }
                break;
            case EnemyState.Hurt:
                attacking = false;
                enemyRb.velocity = Vector2.zero;
                StopAllCoroutines();
                break;

        }
    }
    private bool CanAttack()
    {
        return Vector2.Distance(transform.position, player.transform.position) < currentStrikeRange;
    }

    protected override void DoStateAction()
    {
        base.DoStateAction();
        switch (currentState)
        {
            case EnemyState.Strike:
                Strike();
                break;
            case EnemyState.PowerStrike:
                StrikeWithPower();
                break;
        }
    }

    protected override void EndState()
    {
        switch (currentState)
        {
            case EnemyState.PowerStrike:
                EndPowerStrike();
                attacking = false;
                break;
            case EnemyState.Strike:
                attacking = false;
                break;
            case EnemyState.Hurt:
                fightStarted = false;
                break;
        }

        base.EndState();

        if (currentState == EnemyState.Shoot || currentState == EnemyState.PowerStrike || currentState == EnemyState.Strike || currentState == EnemyState.Hurt)
        {
            StartCoroutine(BeginNewCircle());
        }
    }
    #region StateMetods
    protected void Strike()
    {
        Collider2D player = Physics2D.OverlapBox(strikePoint.position, new Vector2(strikeRange, strikeRange), 0, enemies);
        if (player != null)
        {
            PlayerController playerControler = player.GetComponent<PlayerController>();
            if (playerControler != null)
                playerControler.ChangeHP(-strikeDamage);
        }
    }

    protected void StrikeWithPower() 
    {
        strikeCollider.enabled = true;
        enemyRb.velocity = transform.right * powerStrikeSpeed;
    }

    protected void EndPowerStrike()
    {
        strikeCollider.enabled = false;
        enemyRb.velocity = Vector2.zero;
    }
    #endregion
    protected override void CheckPlayerInRange()
    {
        if (player == null || isAngry)
            return;

        if (Vector2.Distance(transform.position, player.transform.position) < angerRange)
        {
            isAngry = true;
            if (!fightStarted)
            {
                StopAllCoroutines();
                StartCoroutine(BeginNewCircle());
            }
        }
        else
            isAngry = false;
    }

    private IEnumerator BeginNewCircle()
    {
        if (currentState == EnemyState.Death)
            yield break;

        if (fightStarted)
        {
            //isAngry = false;
            ChangeState(EnemyState.Idle);
            CheckPlayerInRange();
            if (!isAngry)
            {
                fightStarted = false;
                StartCoroutine(ScanForPlayer());
                yield break;
            }
            yield return new WaitForSeconds(waitTime);
        }
        fightStarted = true;
        TurnToPlayer();
        ChooseNextAttackState();
    }
    protected void ChooseNextAttackState()
    {
        int state = Random.Range(0, attackStates.Length);
        ChangeState(attackStates[state]);
    }
    /*
public override void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform palyer = null)
{
    if (currentState == EnemyState.PowerStrike && type != DamageType.Projectile || currentState == EnemyState.Hurt)
        return;

    base.TakeDamage(damage, type, palyer);

    if (currentHp <= maxHp / 2 && !inRage)
    {
        inRage = true;
        ChangeState(EnemyState.Hurt);
    }

}*/

    /*
protected override void TryToDamage(Collider2D enemy)
{
    if (currentState == EnemyState.Idle || currentState == EnemyState.Shoot || currentState == EnemyState.Hurt)
        return;

    base.TryToDamage(enemy);
}
*/
    /*
protected override void ResetState()
{
    base.ResetState();
    enemyAnimator.SetBool(EnemyState.PowerStrike.ToString(), false);
    enemyAnimator.SetBool(EnemyState.Strike.ToString(), false);
}*/

}
