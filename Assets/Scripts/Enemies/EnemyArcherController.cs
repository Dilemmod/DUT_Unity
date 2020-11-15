using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcherController : EnemyControllerBase
{
    protected PlayerController player;
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float arrowSpeed;
    [SerializeField] protected float angerRange;

    protected bool isAngry;
    protected bool attacking;
    protected override void Start()
    {
        base.Start();
        //angerRange = range;
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(ScanForPlayer());
    }
    protected override void Update()
    {
        if (isAngry)
            return;
        base.Update();
    }
    protected void Shoot()
    {
        GameObject arrow = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = transform.right * arrowSpeed;
        arrow.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(arrow, 5f);
    }
    protected IEnumerator ScanForPlayer()
    {
        while(true)
        {
            CheckPlayerInRange();
            yield return new WaitForSeconds(1f);
        }

    }
    protected void CheckPlayerInRange()
    {
        if (player == null|| attacking)
            return;
        if (Vector2.Distance(transform.position, player.transform.position) < angerRange)
        {
            isAngry = true;
            TurnToPlayer();
            ChangeState(EnemyState.Shoot);
        }
        else
            isAngry = false;
    }

    protected void TurnToPlayer()
    {
        if (player.transform.position.x - transform.position.x > 0 && !faceRight)
            Flip();
        else if(player.transform.position.x - transform.position.x < 0 && faceRight)
            Flip();
    }

    protected override void ChangeState(EnemyState state)
    {

        base.ChangeState(state);
        switch (state)
        {
            case EnemyState.Shoot:
                attacking = true;
                enemyRb.velocity = Vector2.zero;
                break;
        }
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
    protected void EndState()
    {
        switch (currentState)
        {
            case EnemyState.Shoot:
                attacking = false;
                break;
        }
        if (isAngry)
            ChangeState(EnemyState.Idle);
    }
    protected void DoStateAction()
    {
        switch (currentState)
        {
            case EnemyState.Shoot:
                Shoot();
                break;
        }
    }
}
