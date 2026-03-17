using UnityEngine;

public enum EnemyState
{
    Idle,
    Moving,
    Attacking,
    Hurt,
    Dead
}

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float detectionRange = 10f;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Components")]
    private Rigidbody2D rb;
    private HealthSystem health;
    private EnemyAnimationController animController;

    [Header("State Management")]
    private EnemyState currentState = EnemyState.Idle;
    private float lastAttackTime = -999f;
    private float distanceToPlayer = float.MaxValue;
    private Vector2 moveDirection = Vector2.zero;

    [Header("Performance")]
    private float distanceCheckInterval = 0.1f;
    private float lastDistanceCheck = 0f;

    #region Unity Lifecycle

    void Start()
    {
        InitializeComponents();
        SubscribeToEvents();
    }

    void Update()
    {
        if (currentState == EnemyState.Dead || player == null) return;

        UpdateDistanceToPlayer();
        UpdateState();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (currentState == EnemyState.Dead) return;

        HandleMovement();
    }

    #endregion

    #region Initialization

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthSystem>();
        animController = GetComponent<EnemyAnimationController>();

        // Auto-find player if not assigned
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void SubscribeToEvents()
    {
        if (health != null)
        {
            health.OnHurt += OnHurt;
            health.OnDeath += OnDeath;
        }
    }

    #endregion

    #region State Management

    private void UpdateDistanceToPlayer()
    {
        if (Time.time - lastDistanceCheck >= distanceCheckInterval)
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.position);
            lastDistanceCheck = Time.time;
        }
    }

    private void UpdateState()
    {
        if (currentState == EnemyState.Hurt || currentState == EnemyState.Attacking)
            return;

        EnemyState previousState = currentState;

        if (distanceToPlayer > detectionRange)
        {
            ChangeState(EnemyState.Idle);
        }
        else if (distanceToPlayer <= attackRange)
        {
            if (CanAttack())
            {
                ChangeState(EnemyState.Attacking);
                StartAttack();
            }
            else
            {
                ChangeState(EnemyState.Idle);
            }
        }
        else
        {
            ChangeState(EnemyState.Moving);
        }

        if (previousState != currentState)
        {
            Debug.Log($"Enemy State Change: {previousState} → {currentState}, Distance: {distanceToPlayer:F2}");
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        // Exit current state
        switch (currentState)
        {
            case EnemyState.Attacking:
                EndAttack();
                break;
        }

        // Enter new state
        currentState = newState;
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        if (currentState != EnemyState.Moving)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // Update movement direction continuously
        moveDirection = GetChaseDirection();
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        FaceDirection();
    }

    private Vector2 GetChaseDirection()
    {
        return (player.position - transform.position).normalized;
    }

    private void FaceDirection()
    {
        if (moveDirection.x != 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveDirection.x), 1f, 1f);
        }
    }

    #endregion

    #region Combat

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    private void StartAttack()
    {
        lastAttackTime = Time.time;
        animController.EnemyAttack();
    }

    private void EndAttack()
    {
        // Called by animation event
    }

    #endregion

    #region Animation

    private void UpdateAnimation()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                animController.EnemyIdle();
                break;
            case EnemyState.Moving:
                animController.EnemyWalk();
                break;
            case EnemyState.Attacking:
                // Animation handled by StartAttack()
                break;
            case EnemyState.Hurt:
                // Animation handled by OnHurt()
                break;
            case EnemyState.Dead:
                // Animation handled by OnDeath()
                break;
        }
    }

    #endregion

    #region Event Handlers

    private void OnHurt()
    {
        ChangeState(EnemyState.Hurt);
        animController.EnemyHurt();
    }

    private void OnDeath()
    {
        ChangeState(EnemyState.Dead);
        rb.linearVelocity = Vector2.zero;
        animController.EnemyDead();
        this.enabled = false;
    }

    #endregion

    #region Public Methods (Animation Events)

    public void EndEnemyAttack()
    {
        if (currentState == EnemyState.Attacking)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    public void EnemyActivateHitbox()
    {
        // Kích hoạt hitbox tấn công
        Debug.Log("Enemy: Activate attack hitbox");
        // TODO: Kích hoạt attack hitbox
    }

    public void EnemyDeactivateHitbox()
    {
        // Tắt hitbox tấn công
        Debug.Log("Enemy: Deactivate attack hitbox");
        // TODO: Tắt attack hitbox
    }

    public void OnHurtStart()
    {
        ChangeState(EnemyState.Hurt);
    }

    public void OnHurtEnd()
    {
        if (currentState == EnemyState.Hurt)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;

        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, detectionRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRange);

        // Movement direction
        if (currentState == EnemyState.Moving && moveDirection != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(origin, moveDirection * 2f);
        }
    }

    #endregion
}
