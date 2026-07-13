using UnityEngine;

/// <summary>
/// 플레이어의 기본 대기(Idle) 상태.
/// 플레이어가 이동하지 않는 상태이며,
/// 입력에 따라 걷기, 달리기, 앉기, 점프 상태로 전환한다.
/// </summary>
public class PlayerIdleState : PlayerGroundState
{
	public PlayerIdleState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// Idle 상태에서는 이동 속도를 0으로 설정
		applySpeed = 0f;

		// 기본 카메라(서있는 시점)로 변경
        player.ChangeCameraStand();

		// 앉기 및 조준 상태 해제
        player.isCrouch = false;
        player.onRifle = false;
    }

	public override void Update()
	{
		base.Update();

		// 상태 전환 검사
		ChangeState();

		// Idle 상태에서도 중력 처리를 위해 이동 로직 실행
        MoveLogic();
    }

	public override void Exit()
	{
		base.Exit();
    }

	/// <summary>
	/// 입력에 따라 다른 상태로 전환한다.
	/// </summary>
	protected override void ChangeState()
	{
		// GroundState의 공통 상태 전환
		// (공중 상태, 경사면 상태)
		base.ChangeState();

		// 이동 입력이 들어온 경우
		if (xInput != 0 || zInput != 0)
		{
			// Shift를 누른 채 이동하면 달리기
			if (Input.GetKeyDown(KeyCode.LeftShift))
				stateMachine.ChangeState(player.runState);

			// 그렇지 않으면 걷기
			else
				stateMachine.ChangeState(player.walkState);
		}

		// Ctrl 입력 시 앉기
		else if (Input.GetKeyDown(KeyCode.LeftControl))
			stateMachine.ChangeState(player.sitState);

		// Space 입력 시 점프
		else if (Input.GetKeyDown(KeyCode.Space))
			stateMachine.ChangeState(player.jumpState);
	}
}
