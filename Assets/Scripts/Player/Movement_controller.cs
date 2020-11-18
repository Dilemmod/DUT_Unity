using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement_controller : MonoBehaviour
{
    public event Action<bool> OnGetHurt = delegate { };

    private Rigidbody2D playerRB;
    private Animator playerAnimator;
    private PlayerController playerController;

    [Header("Horizontal movement")]
    [SerializeField] private float speed;
    private bool faceRight = true;

    [Header("Jumping")]
    [SerializeField] private float jumpForse;
    [SerializeField] private float radius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Rolling")]
    [SerializeField] private Transform topPlayerCheck;
    [SerializeField] private Collider2D topPlayerCollider;
    private bool canStand;
    private bool canRoll = true;

    [Header("Casting")]
    [SerializeField] private Transform swordPoint;
    [SerializeField] private GameObject sword;
    [SerializeField] private float swordSpeed;
    [SerializeField] private int castCost;
    private bool isCasting;

    [Header("LightStrike")]
    [SerializeField] private Transform lightStrikePoint;
    [SerializeField] private int lightDamage;
    [SerializeField] private float lightStrikeRange;
    [SerializeField] private int lightStrikeCost;
    private bool isStrike;

    [Header("PowerStrike")]
    [SerializeField] private Transform PowerStrikePoint;
    [SerializeField] private float PowerStrikeRange;
    [SerializeField] private int PowerDamage;
    [SerializeField] private float minChargeTime;
    [SerializeField] private float maxChargeTime;
    public float MaxChargeTime => maxChargeTime;
    [SerializeField] private int powerStrikeCost;

    [Header("Enemies")]
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float pushForce;
    private float lastHurtTime;
    private float holdTime=0;
    private bool died=false;
    private bool canMove = true;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);
            if (playerAnimator.GetBool("Hit") && grounded && Time.time - lastHurtTime > 0.5f) 
            EndHurt();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(topPlayerCheck.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lightStrikePoint.position, lightStrikeRange);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(PowerStrikePoint.position, PowerStrikeRange);
    }
    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }
    public void Move(float move, bool jump, bool roll)
    {
        if (!canMove)
            return;
        #region Movement
        if (move != 0 && grounded &&!roll&& !isCasting&&!isStrike&& !died)
            playerRB.velocity = new Vector2(speed * move, playerRB.velocity.y);

        if (move > 0 && !faceRight)
            Flip();
        else if (move < 0 && faceRight)
            Flip();
        #endregion
        #region Jumping
      
        if (jump && grounded && canRoll)
        {
            playerAnimator.SetTrigger("Jump");
            playerRB.AddForce(Vector2.up * jumpForse);
            jump = false;
        }
        #endregion
        #region Roll
        canStand = !Physics2D.OverlapCircle(topPlayerCheck.position, radius, whatIsGround);
        if (roll && canRoll && grounded)
        {
            canRoll = false;
            Physics2D.IgnoreLayerCollision(9, 12);
            Physics2D.IgnoreLayerCollision(9,10);
            playerRB.velocity = new Vector2((faceRight == true ? 1 : -1) * 4.0f, playerRB.velocity.y);
            topPlayerCollider.enabled = false;
        }
        else if (!roll && canStand)
            topPlayerCollider.enabled = true;

        #endregion  
        #region Animation
        playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        playerAnimator.SetBool("Roll", (roll && grounded ? true : false));
        playerAnimator.SetBool("CanStand", canStand);
        playerAnimator.SetFloat("AirSpeedY", playerRB.velocity.y);
        playerAnimator.SetBool("Grounded", grounded);
        #endregion
    }
    #region EndAnimation
    private void EndRoll()
    {
        Physics2D.IgnoreLayerCollision(9, 12, false);
        Physics2D.IgnoreLayerCollision(9, 10,false);
        canRoll = true;
    }
    #endregion
    #region Cast
    public void StartCasting()
    {
        if (isCasting||!playerController.ChangeSP(-castCost))
            return;
        isCasting = true;
        playerAnimator.SetBool("Casting",true);
    }
    private void CastFire()
    {
        GameObject swordObject = Instantiate(sword, swordPoint.position, Quaternion.identity);
        swordObject.GetComponent<Rigidbody2D>().velocity = transform.right * swordSpeed;
        swordObject.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(swordObject, 5f);
    }
    private void EndCasting()   
    {
        isCasting = false;
        playerAnimator.SetBool("Casting", false);
    }
    #endregion
    #region Strike
    public void StartStrike(float holdTime)
    {
        if (isStrike)
            return;

        this.holdTime = holdTime;
        if (holdTime >= minChargeTime)
        {
            if (!playerController.ChangeSP(-powerStrikeCost))
                return;
            playerAnimator.SetBool("PowerStrike", true);
        }
        else
        {
            if (!playerController.ChangeSP(-lightStrikeCost))
                return;
            playerAnimator.SetBool("LightStrike", true);
        }

        isStrike = true;
    }
    private void Strike()
    {
        Collider2D[] colidersEnemiesLightAttack = Physics2D.OverlapCircleAll(lightStrikePoint.position, lightStrikeRange,enemies);
        Collider2D[] colidersEnemiesPowerAttack = Physics2D.OverlapCircleAll(PowerStrikePoint.position, PowerStrikeRange, enemies);
        if(playerAnimator.GetBool("LightStrike"))
            for (int i = 0; i < colidersEnemiesLightAttack.Length; i++)
            {
                EnemyControllerBase enemy = colidersEnemiesLightAttack[i].GetComponent<EnemyControllerBase>();
                   enemy.TakeDamage(lightDamage);
                
            }
        else if (playerAnimator.GetBool("PowerStrike"))
            for (int i = 0; i < colidersEnemiesPowerAttack.Length; i++)
            {
                EnemyControllerBase enemy = colidersEnemiesPowerAttack[i].GetComponent<EnemyControllerBase>();
                enemy.TakeDamage(PowerDamage * (holdTime < 0.5 ? 1 : (holdTime < 1 ? 2 : (holdTime < 1.5 ? 3 : (holdTime <= 2 ? 4 : 1)))));
            }
    }
    public void EndStrike()
    {
        if (playerAnimator.GetBool("LightStrike"))
            playerAnimator.SetBool("LightStrike", false);
        else if(playerAnimator.GetBool("PowerStrike"))
            playerAnimator.SetBool("PowerStrike", false);
        isStrike = false;
    }
    #endregion
    public void ResetPlayer()
    {
        playerAnimator.SetBool("Strike", false);
        playerAnimator.SetBool("PowerStrike", false);
        playerAnimator.SetBool("Casting", false);
        playerAnimator.SetBool("Hit", false);
        playerAnimator.SetBool("Death", false);
        isCasting = false;
        isStrike = false;
        canMove = true;
    }
    public void GetHurt(Vector2 position)
    {
        lastHurtTime = Time.time;
        canMove = false;
        OnGetHurt(false);
        Vector2 pushDirection = new Vector2();
        pushDirection.x = position.x > transform.position.x ? -1 : 1;
        pushDirection.y = 1;
        playerAnimator.SetBool("Hit", true);
        playerRB.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    }
    private void EndHurt()
    {
        ResetPlayer();
        OnGetHurt(true);
    }
}
