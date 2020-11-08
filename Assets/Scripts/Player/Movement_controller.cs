using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement_controller : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Animator playerAnimator;
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
    //private bool keyPressed = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(topPlayerCheck.position, radius);
    }
    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }
    public void Move(float move, bool jump, bool roll)
    {
        #region Movement
        if (move != 0 && grounded && canRoll)
            playerRB.velocity = new Vector2(speed * move, playerRB.velocity.y);

        if (move > 0 && !faceRight)
            Flip();
        else if (move < 0 && faceRight)
            Flip();
        #endregion
        #region Jumping
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);
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
        void EndRoll()
        {
            canRoll = true;
        }
        #endregion

    }

}
