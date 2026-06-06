using UnityEngine;

/// <summary>
/// 플레이어의 생존 스탯(체력, 허기, 갈증, 정신력, 추위)을 관리
/// </summary>
public class PlayerStatusManager : SingletonBehaviour<PlayerStatusManager>
{
    private PlayerControll player => PlayerManager.Instance.PlayerController;

    #region 최대 스탯

    [Header("스테이터스 최대치")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxThirst = 100f;
    [SerializeField] private float maxMentality = 100f;
    [SerializeField] private float maxCold = 100f;

    #endregion

    #region 현재 스탯

    private float currentHp;
    private float currentHunger;
    private float currentThirst;
    private float currentMentality;
    private float currentCold;

    #endregion

    #region 감소량

    [Header("스테이터스 감소량")]
    [SerializeField] private float hungerDecreaseRate = 1f;
    [SerializeField] private float thirstDecreaseRate = 1f;
    [SerializeField] private float mentalityDecreaseRate = 1f;
    [SerializeField] private float coldDecreaseRate = 1f;

    #endregion

    #region 상태이상 데미지

    [Header("스테이터스별 데미지")]
    [SerializeField] private float hungerDamage = 15f;
    [SerializeField] private float thirstDamage = 10f;
    [SerializeField] private float mentalityDamage = 10f;
    [SerializeField] private float coldDamage = 20f;

    #endregion

    #region 쿨타임

    [Header("쿨타임")]
    [SerializeField] private float statusCooldown = 1f;

    [SerializeField] private float hungerDamageCooldown = 1f;
    [SerializeField] private float thirstDamageCooldown = 1f;
    [SerializeField] private float mentalityDamageCooldown = 1f;
    [SerializeField] private float coldDamageCooldown = 1f;

    private float statusTimer;

    private float hungerDamageTimer;
    private float thirstDamageTimer;
    private float mentalityDamageTimer;
    private float coldDamageTimer;

    #endregion

    #region 이동속도

    private const float WALK_SPEED = 5f;
    private const float SIT_SPEED = 2f;

    private float runDefaultSpeed = 8f;
    private float runDebugSpeed = 20f;
    private float currentRunSpeed = 8f;

    #endregion

    #region 상태 플래그

    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isHunger;
    [HideInInspector] public bool isThirst;
    [HideInInspector] public bool isTired;
    [HideInInspector] public bool isCold;

    private bool deadTrigger;

    #endregion

    #region Property

    public float CurrentHp => currentHp;
    public float CurrentHpPercent => currentHp / maxHp;

    public float CurrentHunger => currentHunger;
    public float CurrentHungerPercent => currentHunger / maxHunger;

    public float CurrentThirst => currentThirst;
    public float CurrentThirstPercent => currentThirst / maxThirst;

    public float CurrentMentality => currentMentality;
    public float CurrentMentalityPercent => currentMentality / maxMentality;

    public float CurrentCold => currentCold;
    public float CurrentColdPercent => currentCold / maxCold;

    #endregion

    private void Start()
    {
        InitStatus();
    }

    private void Update()
    {
        UpdateStatus();
        CheckDeath();
        DebugSpeedToggle();
    }

    /// <summary>
    /// 스탯 초기화
    /// </summary>
    private void InitStatus()
    {
        currentHp = maxHp;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentMentality = maxMentality;
        currentCold = maxCold;
    }

    /// <summary>
    /// 전체 스탯 업데이트
    /// </summary>
    private void UpdateStatus()
    {
        if (TimeManager.Instance.isPause)
            return;

        statusTimer += Time.deltaTime;

        // 일정 시간마다 스탯 감소
        if (statusTimer >= statusCooldown)
        {
            currentHunger -= hungerDecreaseRate;
            currentThirst -= thirstDecreaseRate;
            currentMentality -= mentalityDecreaseRate;
            currentCold -= coldDecreaseRate;

            statusTimer = 0f;
        }

        HandleHunger();
        HandleThirst();
        HandleMentality();
        HandleCold();
    }

    /// <summary>
    /// 공통 상태이상 처리 함수
    /// 수치가 0 이하이면 일정 주기마다 데미지 적용
    /// </summary>
    private void HandleStatusDamage(
        ref float currentValue,
        ref bool stateFlag,
        ref float timer,
        float damage,
        float cooldown)
    {
        if (currentValue > 0)
        {
            stateFlag = false;
            return;
        }

        currentValue = 0;
        stateFlag = true;

        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            TakeDamage(damage);
            timer = 0f;
        }
    }

    /// <summary>
    /// 허기 상태 처리
    /// </summary>
    private void HandleHunger()
    {
        HandleStatusDamage(
            ref currentHunger,
            ref isHunger,
            ref hungerDamageTimer,
            hungerDamage,
            hungerDamageCooldown);

        // 허기 상태 시 이동속도 감소
        if (isHunger)
        {
            player.runSpeed = 5f;
            player.walkSpeed = 3f;
            player.sitSpeed = 1f;
        }
        else
        {
            player.runSpeed = currentRunSpeed;
            player.walkSpeed = WALK_SPEED;
            player.sitSpeed = SIT_SPEED;
        }
    }

    /// <summary>
    /// 갈증 상태 처리
    /// </summary>
    private void HandleThirst()
    {
        HandleStatusDamage(
            ref currentThirst,
            ref isThirst,
            ref thirstDamageTimer,
            thirstDamage,
            thirstDamageCooldown);
    }

    /// <summary>
    /// 정신력 상태 처리
    /// </summary>
    private void HandleMentality()
    {
        HandleStatusDamage(
            ref currentMentality,
            ref isTired,
            ref mentalityDamageTimer,
            mentalityDamage,
            mentalityDamageCooldown);
    }

    /// <summary>
    /// 추위 상태 처리
    /// </summary>
    private void HandleCold()
    {
        HandleStatusDamage(
            ref currentCold,
            ref isCold,
            ref coldDamageTimer,
            coldDamage,
            coldDamageCooldown);
    }

    /// <summary>
    /// 사망 체크
    /// </summary>
    private void CheckDeath()
    {
        if (currentHp > 0 || deadTrigger)
            return;

        deadTrigger = true;
        player.PlayerDying();
    }

    /// <summary>
    /// 디버그 속도 토글
    /// </summary>
    private void DebugSpeedToggle()
    {
        if (!Input.GetKeyDown(KeyCode.J))
            return;

        currentRunSpeed =
            currentRunSpeed == runDefaultSpeed
            ? runDebugSpeed
            : runDefaultSpeed;
    }

    #region 스탯 회복

    public void AddCurrentHunger(float value)
    {
        currentHunger = Mathf.Clamp(currentHunger + value, 0, maxHunger);
    }

    public void AddCurrentThirst(float value)
    {
        currentThirst = Mathf.Clamp(currentThirst + value, 0, maxThirst);
    }

    public void AddCurrentMentality(float value)
    {
        currentMentality = Mathf.Clamp(currentMentality + value, 0, maxMentality);
    }

    public void AddCurrentCold(float value)
    {
        currentCold = Mathf.Clamp(currentCold + value, 0, maxCold);
    }

    #endregion

    #region 체력

    /// <summary>
    /// 데미지 적용
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            currentHp = 0;
            isDead = true;
        }
    }

    /// <summary>
    /// 체력 회복
    /// </summary>
    public void Heal(float amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
    }

    #endregion
}
