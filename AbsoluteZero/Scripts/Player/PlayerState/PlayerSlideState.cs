using UnityEngine;

public class PlayerSlideState : PlayerState
{
	// 경사면을 따라 미끄러지는 방향
	private Vector3 slideDirection;

	// 플레이어의 미끄러짐 상태를 초기화
	public PlayerSlideState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) 
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 현재 경사면의 미끄러질 방향 계산
		slideDirection = GetSlopeDirection();

		// 미끄러짐 속도 적용
		applySpeed = player.slideSpeed;
	}

	public override void Update()
	{
		base.Update();

		// 경사면을 따라 이동
		SlideLogic();

		// 경사 상태에 따른 상태 전환
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		// 더 이상 급경사 위가 아니라면 일반 이동 상태로 복귀
		if (!player.IsOnSteepSlope())
		{
			if (player.onRifle)
			{
				// 무기 장착 상태에서는 Shift 입력 여부에 따라 대기 또는 달리기 상태로 전환
				if (Input.GetKey(KeyCode.LeftShift))
					stateMachine.ChangeState(player.rifleRunState);
				else
					stateMachine.ChangeState(player.rifleIdleState);
			}
			else
			{
				// 일반 상태에서는 Shift 입력 여부에 따라 대기 또는 달리기 상태로 전환
				if (Input.GetKey(KeyCode.LeftShift))
					stateMachine.ChangeState(player.runState);
				else
					stateMachine.ChangeState(player.idleState);
			}
		}
	}

	private void SlideLogic()
	{
		// 경사면 방향 이동과 중력(Y축)을 함께 적용
		Vector3 finalMove = slideDirection * applySpeed + Vector3.up * player.velocity.y;

		player.characterController.Move(finalMove * Time.deltaTime);
	}

	private Vector3 GetSlopeDirection()
	{
		// 바닥의 법선을 이용하여 경사면을 따라 미끄러질 방향 계산
		if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 1.5f))
		{
			Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
			return slopeDir;
		}

		// 바닥이 감지되지 않으면 이동하지 않음
		return Vector3.zero;
	}
}
