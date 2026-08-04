using UnityEngine;

public class PlayerSitState : PlayerGroundState
{
	// 플레이어의 앉기 상태를 초기화
	public PlayerSitState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 앉은 상태에서는 이동하지 않도록 속도를 0으로 설정
		applySpeed = 0f;

		// 카메라 높이와 플레이어 상태를 앉기로 변경
		player.ChangeCameraCrouch();
		player.isCrouch = true;
	}

	public override void Update()
	{
		base.Update();

		// 앉은 상태에서의 이동 처리
		MoveLogic();

		// 입력에 따른 상태 전환
		ChangeState();
	}

	protected override void ChangeState()
	{
		// 지상 상태에서 공통으로 처리하는 상태 전환
		base.ChangeState();

		// Ctrl 키를 놓으면 기본 대기 상태로 복귀
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			// 무기를 장착 중이면 조준 대기 상태, 아니면 일반 대기 상태
			if (player.onRifle)
				stateMachine.ChangeState(player.rifleIdleState);
			else
				stateMachine.ChangeState(player.idleState);
		}
		// 이동 입력이 들어오면 앉아서 걷기 상태로 전환
		else if (xInput != 0 || zInput != 0)
		{
			stateMachine.ChangeState(player.sitWalkState);
		}
	}
}
