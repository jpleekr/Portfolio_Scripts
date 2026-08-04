using UnityEngine;

public abstract class PlayerState
{
    // 상태를 제어하는 플레이어
    protected PlayerControll player;

    // 플레이어 상태 머신
    protected PlayerStateMachine stateMachine;

    // 현재 상태와 연결된 애니메이션 파라미터 이름
    protected string animBoolName;

    // 이동 입력값
    protected float xInput;
    protected float zInput;

    // 현재 상태에서 사용할 이동 속도
    protected float applySpeed;

    // 상태에 필요한 객체들을 초기화
    public PlayerState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        // 상태 진입 시 실행
        // player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        // 플레이어 입력 갱신
        GetInput();

        // 중력 적용
        Gravity();
    }

    public virtual void Exit()
    {
        // 상태 종료 시 실행
        // player.anim.SetBool(animBoolName, false);
    }

    // 플레이어 입력을 읽어 저장
    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
    }

    // 각 상태에서 구현할 상태 전환 로직
    protected abstract void ChangeState();

    // CharacterController 환경에서 사용할 중력 처리
    protected void Gravity()
    {
        // 바닥에 닿아 있고 아래로 떨어지는 중이라면 캐릭터를 지면에 고정
        if (player.characterController.isGrounded && player.velocity.y < 0)
        {
            player.velocity.y = -2f;
        }
        else
        {
            // 공중에서는 중력을 지속적으로 적용
            player.velocity.y += player.gravity * Time.deltaTime;

            // 낙하 속도를 최대값으로 제한
            if (player.velocity.y < player.maxGravity)
                player.velocity.y = player.maxGravity;
        }
    }
}
