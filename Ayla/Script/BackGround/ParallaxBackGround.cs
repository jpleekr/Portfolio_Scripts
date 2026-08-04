using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBackGround : MonoBehaviour
{
    // 메인 카메라
    private Camera mainCam;

    // 배경의 이동 비율 (0 ~ 1)
    [SerializeField] private float parallaxEffect;

    // 현재 배경의 기준 위치
    private float postionX;

    // 배경 한 장의 가로 길이
    private float paddingX;

    void Start()
    {
        // 메인 카메라 참조
        mainCam = Camera.main;

        // 스프라이트의 가로 길이 저장
        paddingX = GetComponent<SpriteRenderer>().bounds.size.x;

        // 초기 위치 저장
        postionX = transform.position.x;
    }

    void Update()
    {
        // 카메라 이동량에 따라 배경을 재배치하기 위한 기준 거리
        float distanceMoved = mainCam.transform.position.x * (1 - parallaxEffect);

        // 패럴랙스 효과를 적용한 실제 배경 이동 거리
        float distanceToMove = mainCam.transform.position.x * parallaxEffect;

        // 카메라 이동 비율만큼 배경 이동
        transform.position = new Vector3(postionX + distanceToMove, transform.position.y);

        // 카메라가 배경 범위를 벗어나면 오른쪽으로 배경 위치 재배치
        if (distanceMoved > postionX + (paddingX * 2))
            postionX += (paddingX * 3);

        // 카메라가 반대 방향으로 벗어나면 왼쪽으로 배경 위치 재배치
        else if (distanceMoved < postionX - (paddingX * 2))
            postionX -= (paddingX * 3);
    }
}
