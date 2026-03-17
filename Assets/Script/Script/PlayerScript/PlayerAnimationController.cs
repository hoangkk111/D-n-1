using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private string currentState;

    private const string PLAYER_IDLE = "PlayerIdle";
    private const string PLAYER_WALK = "PlayerWalk";
    private const string PLAYER_JUMP = "PlayerJump";
    private const string PLAYER_ATTACK = "PlayerAttack1";
    private const string PLAYER_HURT = "PlayerHurt";
    private const string PLAYER_DEAD = "PlayerDead";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayerIdle() => PlayAnimation(PLAYER_IDLE);
    public void PlayerWalk() => PlayAnimation(PLAYER_WALK);
    public void PlayerJump() => PlayAnimation(PLAYER_JUMP);
    public void PlayerAttack() => PlayAnimation(PLAYER_ATTACK);
    public void PlayerHurt() => PlayAnimation(PLAYER_HURT);
    public void PlayerDead() => PlayAnimation(PLAYER_DEAD);

    private void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        animator.CrossFade(newState, 0.1f); // chuyển mượt
        currentState = newState;
    }
}
