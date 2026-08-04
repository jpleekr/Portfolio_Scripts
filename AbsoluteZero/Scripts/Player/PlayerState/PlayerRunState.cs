using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
	// 달리기 상태 생성자
	public PlayerRunState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 이동 속도를 달리기 속도로 설정
		applySpeed = player.runSpeed;

		// 현재 이동 상황에 맞는 발소리 선택
        MoveSoundChoice();
    }

	public override void Update()
	{
		base.Update();

		// 플레이어 이동 처리
		MoveLogic();

		// 입력에 따른 상태 전환
		ChangeState();

		// 이동 중 발소리 재생
        MoveSoundApply();
    }

	public override void Exit()
	{
		base.Exit();

		// 현재는 추가 종료 처리 없음
    }

	protected override void ChangeState()
	{
		// 부모 클래스의 공통 상태 전환 처리
		base.ChangeState();

		// Shift를 떼면 이동 여부에 따라 걷기 또는 대기 상태로 전환
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			// 이동 입력이 있으면 걷기 상태
			if (xInput != 0 || zInput != 0)
				stateMachine.ChangeState(player.walkState);

			// 이동 입력이 없으면 대기 상태
			else if (xInput == 0 && zInput == 0)
				stateMachine.ChangeState(player.idleState);
		}
		// Ctrl을 누르면 앉기 상태
		else if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			stateMachine.ChangeState(player.sitState);
		}
		// Space를 누르면 점프 상태
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			stateMachine.ChangeState(player.jumpState);
		}
	}
}
