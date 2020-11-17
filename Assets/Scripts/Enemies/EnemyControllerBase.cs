using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class EnemyControllerBase : MonoBehaviour
{
    protected Rigidbody2D enemyRb;
    protected Animator enemyAnimator;
    protected EnemyState currentState;
    //[SerializeField] protected Collider2D enemyCollider;
    [Header("HP")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected Canvas canvas; 
    [SerializeField] protected Slider hpSlider;
    protected int currentHp;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    protected Vector2 startPoint;
    protected bool faceRight = true;

    [Header("StateChanges")]
    [SerializeField] private float maxStateTime;
    [SerializeField] private float minStateTime;
    [SerializeField] private EnemyState[] availableState;
    protected float lastStateChange;
    protected float timeToNextChange;
    
    [Header("Damage dealer")]
    [SerializeField] private DamageType collisionDamageType;
    [SerializeField] protected int collisionDamage;
    [SerializeField] protected float collisionTimeDelay;
    private float lastDamageTime;

    #region UnityMethods
    protected virtual void Start()
    {
        startPoint = transform.position;
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;

    }
    protected virtual void Update()
    {
        if (Time.time - lastStateChange > timeToNextChange)
            GetRandomState();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        TryToDamage(collision.collider);
    }
    public virtual void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Death)
            return;

        currentHp -= damage;
        ChangeState(EnemyState.Hit);
        if(hpSlider!=null)
        hpSlider.value = currentHp;
        Debug.Log(String.Format("Enemy {0} take damage {1} and his currentHp = {2}", gameObject, damage, currentHp));
        if (currentHp <= 0)
        {
            currentHp = 0;
            if(canvas!=null)
                Destroy(canvas,1f);
            ChangeState(EnemyState.Death);
        }
    }
    protected virtual void TryToDamage(Collider2D enemy)
    {
        if ((Time.time - lastDamageTime) < collisionTimeDelay)
            return;
        PlayerController player = enemy.GetComponent<PlayerController>();
        if (player != null)
            player.TakeDamage(collisionDamage, collisionDamageType, transform);
            
    }
    protected virtual void FixedUpdate()
    {
        if (IsGroundEnding())
            Flip();

        if(currentState == EnemyState.Move)
            Move();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, 0.5f, 0));
    }
    protected virtual void ResetState()
    {
        enemyAnimator.SetBool(EnemyState.Move.ToString(), false);
        enemyAnimator.SetBool(EnemyState.Death.ToString(), false);
    }
    protected void GetRandomState()
    {
        if (currentState == EnemyState.Death)
            return;
        
        int state = Random.Range(0, availableState.Length);
         
        if (currentState == EnemyState.Idle && availableState[state] == EnemyState.Idle)
            GetRandomState();
        
        timeToNextChange = Random.Range(minStateTime, maxStateTime);

        ChangeState(availableState[state]);
    }
    protected virtual void ChangeState(EnemyState state)
    {
        if (currentState == EnemyState.Death)
            return;
        ResetState();   
        if (currentState!=EnemyState.Idle)
            enemyAnimator.SetBool(currentState.ToString(), false);
        if (state != EnemyState.Idle)
            enemyAnimator.SetBool(state.ToString(),true);
        currentState = state;
        lastStateChange = Time.time;

        switch (currentState)
        {
            case EnemyState.Idle:
                enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Death:
                enemyAnimator.SetTrigger("Death");
                enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Hit:
                enemyAnimator.SetTrigger("Hit");
                enemyRb.velocity = Vector2.zero;
                //StopAllCoroutines();
                break;
        }
    }
    protected virtual void onDeath()
    {
        enemyAnimator.enabled = false;
        enemyRb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }

    protected virtual void Move()
    {
        enemyRb.velocity = transform.right * new Vector2(speed, enemyRb.velocity.y);
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
        if(canvas!=null)
        canvas.transform.Rotate(0,180,0);
    }

    private bool IsGroundEnding()
    {
        return !Physics2D.OverlapPoint(groundCheck.position, whatIsGround);
    }
    #endregion
}

public enum EnemyState
{
    Idle,
    Move,
    Shoot,
    Strike,
    PowerStrike,
    Hit,
    Death,
}



