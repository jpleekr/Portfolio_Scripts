using UnityEngine;

public class PlayerStateMachine
{
    // 현재 실행 중인 플레이어 상태
    private PlayerState currentState;

    // 상태 머신을 초기 상태로 설정
    public void InitState(PlayerState newState)
    {
        currentState = newState;

        // 초기 상태 진입
        currentState.Enter();
    }

    // 현재 상태를 종료하고 새로운 상태로 전환
    public void ChangeState(PlayerState newState)
    {
        // 기존 상태 종료
        currentState.Exit();

        // 새로운 상태로 변경
        currentState = newState;

        // 새로운 상태 진입
        currentState.Enter();
    }

    // 현재 상태의 로직을 실행
    public void Update()
    {
        currentState.Update();
    }
}
