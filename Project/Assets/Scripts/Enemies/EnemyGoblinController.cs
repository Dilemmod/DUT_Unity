using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblinController : EnemyControllerBase
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
    /*
    protected override void AttackStates()
    {
        attackStates.Add(EnemyState.Strike);
        attackStates.Add(EnemyState.PowerStrike);
    }
    public override void TakeDamage(int damage)
    {
        inRage = true;
        base.TakeDamage(damage);
    }
    */
    /*
    [Header("Strike2")]
    [SerializeField] private Transform strikePoint2;
    [SerializeField] private int strikeDamage2;
    [SerializeField] private float strikeRange2;
    [SerializeField] private LayerMask enemies2;


    protected override void StrikeWithPower()
    {
        Collider2D player = Physics2D.OverlapBox(strikePoint2.position, new Vector2(strikeRange2, strikeRange2), 0, enemies2);
        if (player != null)
        {
            PlayerController playerControler = player.GetComponent<PlayerController>();
            if (playerControler != null)
                playerControler.TakeDamage(strikeDamage2);
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color= Color.green;
        Gizmos.DrawWireCube(strikePoint2.position, new Vector3(strikeRange2, strikeRange2, 0));
    }*/
}
