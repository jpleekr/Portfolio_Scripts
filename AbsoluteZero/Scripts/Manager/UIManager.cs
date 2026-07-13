using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 게임의 UI를 전체적으로 관리하는 싱글톤 매니저.
/// 플레이어 UI, 메뉴 UI, 아이템 미리보기, 기록창,
/// 진행바, 페이드 효과 등을 담당한다.
/// </summary>
public class UIManager : SingletonBehaviour<UIManager>
{
    // 현재 메뉴가 열려있는지 여부
    public bool inMenu;

    #region Player UI

    [Header("PlayerUI")]

    // 플레이 중 표시되는 HUD
    [SerializeField] private GameObject playerUICanvas;

    // 상호작용 가능한 오브젝트 이름
    [SerializeField] private TextMeshProUGUI uiSelectItem;

    // 진행바 UI
    [SerializeField] private GameObject uiProgress;
    [SerializeField] private Image uiProgressBar;

    #endregion

    #region Menu UI

    [Header("ItemPreviewMenuUI")]

    // 메뉴 캔버스
    [SerializeField] private GameObject menuUICanvas;

    // 메뉴 패널
    [SerializeField] private GameObject menuPanel;

    // 메뉴 제목
    [SerializeField] private TextMeshProUGUI menuTitle;

    // 아이템 미리보기 정보
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewLore;

    // 마우스 조작 안내
    [SerializeField] private TextMeshProUGUI menuItemMouseLeft;
    [SerializeField] private TextMeshProUGUI menuItemMouseRight;

    // 기록 패널
    [SerializeField] private GameObject recordPanel;

    // 기록 UI
    [SerializeField] private TextMeshProUGUI totalSurvivedTime;
    [SerializeField] private TextMeshProUGUI totalTraveledDistance;
    [SerializeField] private TextMeshProUGUI totalSleepTime;
    [SerializeField] private TextMeshProUGUI totalEatFood;
    [SerializeField] private TextMeshProUGUI totalDrinkWater;
    [SerializeField] private TextMeshProUGUI totalGunFire;
    [SerializeField] private TextMeshProUGUI totalHitCount;
    [SerializeField] private TextMeshProUGUI totalSuccHunt;

    // 메뉴 버튼
    [SerializeField] private Button menuBackBtn;
    [SerializeField] private Button menuAcceptBtn;

    // 수면 시간 입력
    [SerializeField] private Text menuSleepTime;

    // 화면 페이드 이미지
    [SerializeField] private Image fadeImage;

    // 아이템 프리뷰가 생성될 위치
    public Transform menuItemPreviewPos;

    #endregion

    #region Item Menu

    [Header("ItemMenuUI")]

    // 아이템 상세 정보
    [SerializeField] private TextMeshProUGUI menuItemName;
    [SerializeField] private TextMeshProUGUI menuItemLore;

    #endregion

    #region Fade

    [Header("Fade In/Out Setting")]

    // 페이드 지속 시간
    [SerializeField] float fadeDuration = 1f;

    // 현재 실행 중인 페이드 코루틴
    private Coroutine currentFade;

    // 페이드 방향
    private enum FadeDirection { In, Out }

    #endregion

    private void Start()
    {
        // 시작 시 메뉴는 닫혀있는 상태
        inMenu = false;
    }

    private void Update()
    {
        // 디버그용 기록창 열기
        if (Input.GetKeyDown(KeyCode.O))
        {
            RecordMenuOpen();
        }

        // 디버그용 메뉴 닫기
        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseMenu();
        }
    }

    protected override void Init()
    {
        base.Init();
    }

    /// <summary>
    /// 마우스 커서를 표시하거나 숨긴다.
    /// </summary>
    public void CursorVisible(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// 플레이어 HUD 표시 여부 설정
    /// </summary>
    private void SetPlayerUICanvas(bool value)
    {
        playerUICanvas.SetActive(value);
    }

    /// <summary>
    /// 메뉴 UI 표시 여부 설정
    /// </summary>
    private void SetMenuUICanvas(bool value)
    {
        menuUICanvas.SetActive(value);
        inMenu = value;
    }

    /// <summary>
    /// 상호작용 가능한 아이템 이름을 표시한다.
    /// </summary>
    public void FocusInItem(string itemName)
    {
        uiSelectItem.text = itemName;
        uiSelectItem.gameObject.SetActive(true);
    }

    /// <summary>
    /// 아이템 이름 표시를 제거한다.
    /// </summary>
    public void FocusOutItem()
    {
        uiSelectItem.text = "";
        uiSelectItem.gameObject.SetActive(false);
    }

    /// <summary>
    /// 진행바를 표시하고 진행률을 갱신한다.
    /// </summary>
    public void ShowProgress(float pct)
    {
        uiProgress.SetActive(true);
        uiProgressBar.fillAmount = pct;
    }

    /// <summary>
    /// 진행바를 숨긴다.
    /// </summary>
    public void HideProgress()
    {
        uiProgress.SetActive(false);
    }

    /// <summary>
    /// 침대 메뉴를 연다.
    /// </summary>
    public void BedMenuOpen(ObjectBed bed)
    {
        SetPlayerUICanvas(false);
        CursorVisible(true);
        PlayerManager.Instance.SetPlayerFreeze(true);

        // 모든 UI를 끈 후 필요한 UI만 활성화
        MenuElementAllDisable();

        menuTitle.gameObject.SetActive(true);
        menuAcceptBtn.gameObject.SetActive(true);

        // 확인 버튼 클릭 시 잠자기 실행
        menuAcceptBtn.onClick.AddListener(() => StartCoroutine(bed.Sleep(int.Parse(menuSleepTime.text))));

        menuBackBtn.gameObject.SetActive(true);
        menuSleepTime.gameObject.SetActive(true);

        menuTitle.text = "Go to bed";

        SetMenuUICanvas(true);
    }

    /// <summary>
    /// 아이템 획득 메뉴를 연다.
    /// </summary>
    public void ItemPickupMenuOpen()
    {
        MenuElementAllDisable();

        SetPlayerUICanvas(false);
        PlayerManager.Instance.SetPlayerFreeze(true);

        SetMenuUICanvas(true);

        itemPreviewName.gameObject.SetActive(true);
        itemPreviewLore.gameObject.SetActive(true);
        menuItemMouseLeft.gameObject.SetActive(true);
        menuItemMouseRight.gameObject.SetActive(true);
    }

    /// <summary>
    /// 아이템 미리보기 정보를 갱신한다.
    /// </summary>
    public void ItemPickupMenuLoreUpdate(PickupItemData pItem)
    {
        itemPreviewName.text = pItem.itemName;
        itemPreviewLore.text = pItem.itemLore;
    }

    /// <summary>
    /// 아이템 상세 정보를 갱신한다.
    /// </summary>
    public void MenuItemLoreUpdate(PickupItemData pItem)
    {
        menuItemName.text = pItem.itemName;
        menuItemLore.text = pItem.itemLore;
    }

    /// <summary>
    /// 현재 열려있는 메뉴를 닫는다.
    /// </summary>
    public void CloseMenu()
    {
        // 등록된 버튼 이벤트 제거
        menuAcceptBtn.onClick.RemoveAllListeners();

        SetMenuUICanvas(false);
        SetPlayerUICanvas(true);

        CursorVisible(false);
        PlayerManager.Instance.SetPlayerFreeze(false);
    }

    /// <summary>
    /// 메뉴에 포함된 모든 UI 요소를 비활성화한다.
    /// </summary>
    public void MenuElementAllDisable()
    {
        menuTitle.gameObject.SetActive(false);
        itemPreviewName.gameObject.SetActive(false);
        itemPreviewLore.gameObject.SetActive(false);
        menuItemMouseLeft.gameObject.SetActive(false);
        menuItemMouseRight.gameObject.SetActive(false);
        menuBackBtn.gameObject.SetActive(false);
        menuAcceptBtn.gameObject.SetActive(false);
        recordPanel.gameObject.SetActive(false);
        menuSleepTime.gameObject.SetActive(false);
    }

    /// <summary>
    /// 플레이 기록을 UI에 출력한다.
    /// </summary>
    public void RecordMenuOpen()
    {
        int day = 0;
        int hour = 0;
        int min = 0;
        float sec = GameRecode.instance.totalSurvivedTime;

        // 초 -> 분 -> 시간 -> 일 변환
        if (GameRecode.instance.totalSurvivedTime > 60)
        {
            min += (int)(sec / 60);
            sec = sec % 60;

            if (min > 60)
            {
                hour += min / 60;
                min = min % 60;

                if (hour > 24)
                {
                    day += hour / 24;
                    hour = hour % 24;
                }
            }
        }

        // 각 기록 UI 갱신
        totalSurvivedTime.text = "생존한 시간 : " + day + "D " + hour + "H " + min + "M " + Mathf.Floor(sec) + "S";
        totalTraveledDistance.text = "이동한 거리 : " + Mathf.RoundToInt(GameRecode.instance.totalTraveledDistance * 100f) / 100f + "M";
        totalSleepTime.text = "잠을 잔 시간 : " + GameRecode.instance.totalSleepTime;
        totalEatFood.text = "회복한 배고픔 : " + GameRecode.instance.totalEatFood;
        totalDrinkWater.text = "회복한 갈증 : " + GameRecode.instance.totalDrinkWater;
        totalGunFire.text = "사격을 실행 한 횟수 : " + GameRecode.instance.totalGunFire;
        totalHitCount.text = "명중한 횟수 : " + GameRecode.instance.totalShootHit;
        totalSuccHunt.text = "사냥에 성공한 횟수 : " + GameRecode.instance.totalSuccessHunt;
    }

    /// <summary>
    /// 화면을 서서히 밝게 만든다.
    /// </summary>
    public void FadeIn()
    {
        StartFade(FadeDirection.In);
    }

    /// <summary>
    /// 화면을 서서히 어둡게 만든다.
    /// </summary>
    public void FadeOut()
    {
        StartFade(FadeDirection.Out);
    }

    /// <summary>
    /// 기존 페이드가 실행 중이면 종료 후 새로운 페이드를 시작한다.
    /// </summary>
    private void StartFade(FadeDirection direction)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeRoutine(direction));
    }

    /// <summary>
    /// 페이드 효과를 실행하는 코루틴.
    /// Image의 Alpha 값을 변경하여 화면을 자연스럽게 전환한다.
    /// </summary>
    private IEnumerator FadeRoutine(FadeDirection direction)
    {
        float time = 0f;
        Color color = fadeImage.color;

        float startAlpha = (direction == FadeDirection.In) ? 1f : 0f;
        float endAlpha = (direction == FadeDirection.In) ? 0f : 1f;

        color.a = startAlpha;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / fadeDuration);

            color.a = Mathf.Lerp(startAlpha, endAlpha, t);

            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        currentFade = null;
    }
}
