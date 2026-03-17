using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private string currentState;

    private const string ENEMY_IDLE = "EnemyIdle";
    private const string ENEMY_WALK = "EnemyWalk";
    private const string ENEMY_ATTACK = "EnemyAttack";
    private const string ENEMY_HURT = "EnemyHurt";
    private const string ENEMY_DEAD = "EnemyDead";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void EnemyIdle() => PlayAnimation(ENEMY_IDLE);
    public void EnemyWalk() => PlayAnimation(ENEMY_WALK);
    public void EnemyAttack() => PlayAnimation(ENEMY_ATTACK);
    public void EnemyHurt() => PlayAnimation(ENEMY_HURT);
    public void EnemyDead() => PlayAnimation(ENEMY_DEAD);

    private void PlayAnimation(string newState)
    {
        if (currentState == newState) return;

        Debug.Log($"Enemy Animation: {currentState} → {newState}");
        animator.CrossFade(newState, 0.1f);
        currentState = newState;
    }

    public bool IsPlaying(string animName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
}
