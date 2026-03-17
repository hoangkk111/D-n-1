using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    //[SerializeField] public Vector2 groundCheckSide = new Vector2(0.5f, 0.1f);
    public bool isGround = true;

    [Header("Jump Settings")]
    public int maxJumpCount = 2;
    public int jumpCount = 0;
    public bool isJumping = false;
    [Range(0f, 100f)] public float jumpForce = 5f;

    [Header("Movement Settings")]
    [Range(0f, 100f)] public float speed = 5f;
    public bool canMove = true;
    float timePressMoveLeft = -1f;
    float timePressMoveRight = -1f;
    public float directionX = 0f;
    public MoveDirection moveDirection = MoveDirection.None;

    [Header("Attack Settings")]
    public GameObject attackHitbox;
    public float attackDuration = 0.2f;
    public bool isAttacking = false;

    [Header("Component Settings")]
    private Rigidbody2D rb;
    public Animator anim;
    private Transform playerTransform;
    //private Animation animation;

    [Header("Health System")]
    private HealthSystem health;


    [SerializeField] private PlayerAnimationController animController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //animation = GetComponent<Animation>();

        health = GetComponent<HealthSystem>();
        health.OnHurt += () => animController.PlayerHurt();
        health.OnDeath += () => Die();
    }

    void Update()
    {
        #region Add
        var input = InputHandler.Instance;
        #endregion

        #region Logic
        //isGround = Physics2D.OverlapBox(groundCheck.position, groundCheckSide, 0f, groundLayer);
        //bool wasGrounded = isGround;

        if (jumpCount > 0) isJumping = true;

        if (input.PressAttack)
        {
            isAttacking = true;
            //canMove = false;
            //canJump = false;
        }

        PlayerJump();// Gọi PlayerJump sau khi xử lý logic

        isJumping = jumpCount > 0;// Cập nhật isJumping dựa trên jumpCount không phụ thuộc vào isGrounded
        #endregion

        #region Move
        if (input.HoldRight && input.HoldLeft && canMove)
        {
            moveDirection = (timePressMoveLeft > timePressMoveRight) ? MoveDirection.Left : MoveDirection.Right;
            //Debug.Log("HoldRight && HoldLeft");
        }
        else if (input.HoldLeft && canMove)
        {
            moveDirection = MoveDirection.Left;
            //Debug.Log("HoldLeft");
        }
        else if (input.HoldRight && canMove)
        {
            moveDirection = MoveDirection.Right;
            //Debug.Log("HoldRight");
        }
        else
        {
            moveDirection = MoveDirection.None;
            //Debug.Log("Not Hold");
        }
        #endregion

        #region Anim
        if (isAttacking)
        {
            animController.PlayerAttack();
        }
        else if (isJumping)
        {
            animController.PlayerJump();
        }
        else if (moveDirection != MoveDirection.None)
        {
            animController.PlayerWalk();
        }
        else
        {
            animController.PlayerIdle();
        }
        #endregion

        #region Debug

       

        #endregion

    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocityY);
            return;
        }

        if (moveDirection == MoveDirection.Left && canMove)
            directionX = -1f;
        else if (moveDirection == MoveDirection.Right && canMove)
            directionX = 1f;
        else
            directionX = 0f;

        rb.linearVelocity = new Vector2(directionX * speed, rb.linearVelocity.y);

        if (directionX != 0f)
            FaceDirection();

        CheckGround();
    }
    private void FaceDirection()
    {
        if (directionX < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (directionX > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    private void PlayerJump()
    {
        var input = InputHandler.Instance;

        if (input.PressJump && jumpCount < maxJumpCount && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Đặt lại vận tốc y (trong lực) để nhảy thứ 2 trờ đi có cùng vận tốc với nhảy đầu tiên
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }
    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        bool wasGrounded = isGround;
        isGround = hit.collider != null;

        // Nếu vừa chạm đất
        if (isGround && !wasGrounded)
        {
            isJumping = false;
            jumpCount = 0;
        }
    }
    private void PlayerAttack()
    {
        isAttacking = true;
        attackHitbox.SetActive(true);
        Debug.Log("Player Attack");
    }
    private void EndAttack()
    {
        isAttacking = false;
        //canMove = true;
        //canJump = true
        attackHitbox.SetActive(false);
    }
    private void Die()
    {
        animController.PlayerDead();
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        this.enabled = false; // Vô hiệu hóa script
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGround = true;
    //        isJumping = false;
    //        jumpCount = 0;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGround = false;
    //    }
    //}
    //private void OnDrawGizmos()
    //{
    //    if (groundCheck == null) return;
    //    if (isGround) { Gizmos.color = Color.green; }
    //    else { Gizmos.color = Color.yellow; }
    //    Gizmos.DrawWireCube(groundCheck.position, groundCheckSide);

    //}
}
