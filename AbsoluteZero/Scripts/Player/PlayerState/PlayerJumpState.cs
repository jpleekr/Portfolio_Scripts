/// <summary>
/// 플레이어의 점프 상태.
/// 점프 시작 시 위쪽 속도를 부여하며,
/// 상승이 끝나면 공중(Air) 상태로 전환한다.
/// </summary>
public class PlayerJumpState : PlayerState
{
	public PlayerJumpState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 점프 시작
		JumpLogic();
	}

	public override void Update()
	{
		base.Update();

		// 점프 중 이동 및 중력 적용
		player.characterController.Move(player.velocity * Time.deltaTime);

		// 상태 전환 검사
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	/// <summary>
	/// 점프가 최고점에 도달하여 하강하기 시작하면
	/// Air 상태로 전환한다.
	/// </summary>
	protected override void ChangeState()
	{
		if (player.velocity.y < 0)
			stateMachine.ChangeState(player.airState);
	}

	/// <summary>
	/// 점프에 필요한 초기 위쪽 속도를 계산하여 적용한다.
	/// 또한 점프 효과음을 재생한다.
	/// </summary>
	private void JumpLogic()
	{
		// 목표 점프 높이에 도달하기 위한 초기 속도 계산
		player.velocity.y = Mathf.Sqrt(player.jumpForce * 2f * -player.gravity);

		// 점프 효과음 재생
        SoundManager.Instance.PlayFootstep(SoundManager.FootstepType.Snow);
    }
}
