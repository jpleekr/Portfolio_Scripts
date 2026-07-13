using UnityEngine;

/// <summary>
/// 플레이어가 공중에 있는 상태를 관리하는 State.
/// 점프, 낙하 중 이동과 착지 시 낙하 피해를 처리한다.
/// </summary>
public class PlayerAirState : PlayerState
{
	// 낙하 시작 위치
	private Vector3 FallingStartPos;

	// 착지 위치
	private Vector3 FallingEndPos;

	public PlayerAirState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		// 공중 상태에 진입한 위치를 저장
		// 이후 착지 시 낙하 높이를 계산하는 데 사용된다.
		FallingStartPos = player.transform.position;
	}

	public override void Update()
	{
		base.Update();

		// 중력 및 이동 속도를 적용하여 플레이어 이동
		player.characterController.Move(player.velocity * Time.deltaTime);

		// 상태 전환 검사
		ChangeState();

		Debug.Log("에어 상태");
	}

	public override void Exit()
	{
		base.Exit();

		// 착지 후 수평 이동 속도 초기화
		player.velocity.x = 0;
		player.velocity.z = 0;

		// 착지 위치 저장
		FallingEndPos = player.transform.position;

		// 낙하 높이 계산
		float height = FallingStartPos.y - FallingEndPos.y;

		// 일정 높이 이상 낙하했을 경우 낙하 데미지 적용
		if (height > player.fallingHight)
		{
			PlayerStatusManager.Instance.TakeDamage(hight * player.fallDamageRate);
		}
	}

	/// <summary>
	/// 공중 상태에서 다른 상태로 전환한다.
	/// 바닥에 착지하면 Idle 상태로 변경한다.
	/// </summary>
	protected override void ChangeState()
	{
		// 바닥에 착지했는지 확인
		if (player.characterController.isGrounded)
		{
			// 착지 시 발소리 재생
			SoundManager.Instance.PlayFootstep(SoundManager.FootstepType.Snow);

			Debug.Log("에어 사운드 오류");

			// 현재 무기 장착 여부에 따라 Idle 상태 결정
			if (player.onRifle)
				stateMachine.ChangeState(player.rifleIdleState);
			else
				stateMachine.ChangeState(player.idleState);
		}
	}
}
