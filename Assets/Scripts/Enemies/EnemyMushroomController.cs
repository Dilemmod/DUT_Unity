using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomController : EnemyControllerBase
{
    [SerializeField] private int damage;
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
            yield return new WaitForSeconds(0.5f);
        }

    }
    protected void CheckPlayerInRange()
    {
        if (player == null)
            return;
        if (Vector2.Distance(transform.position, player.transform.position) < angerRange)
        {
            Debug.Log("Boom");
            ChangeState(EnemyState.Death);
        }
    }

    private void BeginBoom()
    {
        player.ChangeHP(-damage);
       // player.transform.position = new Vector2(player.transform.position.x - 0.4f, player.transform.position.y);
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
