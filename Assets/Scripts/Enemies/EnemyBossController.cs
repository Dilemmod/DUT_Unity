using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyBossController : EnemyArcherController
{
    [Header("Other")]
    [SerializeField] private SpriteRenderer enemySpryte;
    protected bool inRage;
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
    protected List<EnemyState> attackStates = new List<EnemyState>();

    #region UnityMethods
    protected override void Start()
    {
        base.Start();
        AttackStates();
    }
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
    protected virtual void AttackStates()
    {
        attackStates.Add(EnemyState.Strike);
        attackStates.Add(EnemyState.PowerStrike);
        attackStates.Add(EnemyState.Shoot);
    }
    #endregion
    #region State
    protected override void ChangeState(EnemyState state)
    {
        if (currentState == state)
            return;
        switch (state)
        {
            case EnemyState.PowerStrike:
            case EnemyState.Strike:
                attacking = true;
                currentStrikeRange = state == EnemyState.Strike ? strikeRange : powerStrikeRange;
                enemyRb.velocity = Vector2.zero;
                if (!CanAttack())
                {
                    stateOnHold = state;
                    state = EnemyState.Move;
                }
                break;
            case EnemyState.Hit:
                attacking = false;
                enemyAnimator.SetTrigger("Hit");
                enemyRb.velocity = Vector2.zero;
               // StopAllCoroutines();
                break;
        }
        base.ChangeState(state);
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
        base.EndState();
        switch (currentState)
        {
            case EnemyState.PowerStrike:
                EndPowerStrike();
                attacking = false;
                break;
            case EnemyState.Strike:
                attacking = false;
                break;
            case EnemyState.Hit:
                fightStarted = false;
                break;
        }
            StartCoroutine(BeginNewCircle());
    }
    #endregion
    #region StateMetods

    protected override void TryToDamage(Collider2D enemy)
    {
        if (currentState == EnemyState.Idle)
            return;
        base.TryToDamage(enemy);
    }
    protected void Strike()
    {
        Collider2D player = Physics2D.OverlapBox(strikePoint.position, new Vector2(strikeRange, strikeRange), 0, enemies);
        if (player != null)
        {
            PlayerController playerControler = player.GetComponent<PlayerController>();
            if (playerControler != null)
                playerControler.TakeDamage(strikeDamage);
        }
    }

    protected virtual void StrikeWithPower() 
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
    #region Circle
    private IEnumerator BeginNewCircle()
    {
        if (currentState == EnemyState.Death)
            yield break;

        if (fightStarted)
        {
            //isAngry = false;
            isAngry = false;
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
    protected void ChooseNextAttackState()
    {
        int state = Random.Range(0, attackStates.Count);
        ChangeState(attackStates[state]);
    }
    #endregion
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (currentHp <= maxHp / 2 && !inRage)
        {
            inRage = true;
            powerStrikeRange = 10;
            waitTime = 0.3f;
            enemySpryte.color = Color.red;
            ChangeState(EnemyState.Hit);
        }
    }
    protected override void onDeath()
    {
        if(inRage)
            enemySpryte.color = Color.white;
        base.onDeath();
    }


}
