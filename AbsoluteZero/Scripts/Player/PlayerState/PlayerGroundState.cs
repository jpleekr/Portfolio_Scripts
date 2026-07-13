using UnityEngine;

/// <summary>
/// 플레이어가 지면 위에 있을 때 사용하는 기본 상태.
/// 이동, 발소리, 공중/경사 상태 전환에 필요한 공통 기능을 제공한다.
/// </summary>
public class PlayerGroundState : PlayerState
{
	public PlayerGroundState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Exit()
	{
		base.Exit();

		// 현재 입력 방향을 velocity에 저장하여
		// 공중 상태로 전환되어도 관성을 유지하도록 한다.
		SetCurrentVelocity();
	}

	/// <summary>
	/// 현재 입력 방향과 이동 속도를 velocity에 저장한다.
	/// Ground 상태를 벗어날 때 공중 이동의 초기 속도로 사용된다.
	/// </summary>
	private void SetCurrentVelocity()
	{
		Vector3 inputDir = player.transform.right * xInput + player.transform.forward * zInput;
		inputDir = inputDir.normalized * applySpeed;

		player.velocity.x = inputDir.x;
		player.velocity.z = inputDir.z;
	}

	/// <summary>
	/// 플레이어를 이동시키고 애니메이터의 이동 속도를 갱신한다.
	/// CharacterController를 이용한 지상 이동 로직이다.
	/// </summary>
	protected void MoveLogic()
	{
		// 입력 방향 계산
        Vector3 move = player.transform.right * xInput + player.transform.forward * zInput;
		move = move.normalized * applySpeed;

		// 수평 이동 + 현재 중력(Y축 속도) 적용
		Vector3 finalMove = move + Vector3.up * player.velocity.y;

		// 플레이어 이동
		player.characterController.Move(finalMove * Time.deltaTime);

		// 애니메이션 Blend 값 갱신
        player.anim.SetFloat("ForwardSpeed", applySpeed, 0.2f, Time.deltaTime);
    }

	/// <summary>
	/// 현재 이동 속도에 따라 발소리 재생 간격을 설정한다.
	/// 달리기와 걷기의 발소리 주기를 구분한다.
	/// </summary>
	protected void MoveSoundChoice()
	{
        if (applySpeed == player.runSpeed)
        {
            player.soundDelay = player.RunSoundDelay;
        }
        else
        {
            player.soundDelay = player.walkSoundDelay;
        }
    }

	/// <summary>
	/// 일정 시간이 지나면 발소리를 재생한다.
	/// </summary>
    protected void MoveSoundApply()
    {
		player.soundDelay -= Time.deltaTime;

		if (player.soundDelay <= 0)
		{
			// 발소리 재생
            SoundManager.Instance.PlayFootstep(SoundManager.FootstepType.Snow);

			// 다음 발소리 재생 시간 설정
            MoveSoundChoice();
        }
    }

	/// <summary>
	/// 현재 지형 상태에 따라 다른 상태로 전환한다.
	/// </summary>
    protected override void ChangeState()
	{
		// 지면에서 떨어졌다면 공중 상태
		if (!player.characterController.isGrounded)
			stateMachine.ChangeState(player.airState);

		// 급경사를 밟았다면 슬라이드 상태
		else if (player.IsOnSteepSlope())
			stateMachine.ChangeState(player.slideState);
	}
}
