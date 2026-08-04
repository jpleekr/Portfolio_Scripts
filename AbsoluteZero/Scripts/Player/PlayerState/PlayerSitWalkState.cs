using UnityEngine;

public class PlayerSitWalkState : PlayerGroundState
{
	// 플레이어의 앉아서 걷기 상태를 초기화
	public PlayerSitWalkState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 앉은 상태의 이동 속도 적용
		applySpeed = player.sitSpeed;
	}

	public override void Update()
	{
		base.Update();

		// 앉아서 이동 처리
		MoveLogic();

		// 입력에 따른 상태 전환
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		// 지상 상태에서 공통으로 처리하는 상태 전환
		base.ChangeState();

		// Ctrl 키를 놓으면 앉기 상태를 해제
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			// 무기 장착 여부에 따라 적절한 대기 상태로 전환
			if (player.onRifle)
				stateMachine.ChangeState(player.rifleIdleState);
			else
				stateMachine.ChangeState(player.idleState);
		}
		// 이동 입력이 없어지면 앉기 상태로 전환
		else if (xInput == 0 && zInput == 0)
		{
			stateMachine.ChangeState(player.sitState);
		}
	}
}
