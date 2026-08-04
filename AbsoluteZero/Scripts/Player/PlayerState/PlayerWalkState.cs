using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
	// 플레이어의 걷기 상태를 초기화
	public PlayerWalkState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 걷기 속도 적용
		applySpeed = player.walkSpeed;

		// 걷기 상태에 맞는 발소리 설정
		MoveSoundChoice();
	}

	public override void Update()
	{
		base.Update();

		// 걷기 이동 처리
		MoveLogic();

		// 입력에 따른 상태 전환
		ChangeState();

		// 이동 중 발소리 재생
		MoveSoundApply();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		// 지상 상태에서 공통으로 처리하는 상태 전환
		base.ChangeState();

		// 이동 입력이 없으면 대기 상태로 전환
		if (xInput == 0 && zInput == 0)
		{
			stateMachine.ChangeState(player.idleState);
		}
		// Shift 입력 시 달리기 상태로 전환
		else if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			stateMachine.ChangeState(player.runState);
		}
		// Ctrl 입력 시 앉기 상태로 전환
		else if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			stateMachine.ChangeState(player.sitState);
		}
		// Space 입력 시 점프 상태로 전환
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			stateMachine.ChangeState(player.jumpState);
		}
	}
}
