using UnityEngine;
using UnityEngine.UI;

public class SleepBtn : MonoBehaviour
{
    // 선택 가능한 최대/최소 수면 시간
    [SerializeField] private int maxSleepTime = 12;
    [SerializeField] private int minSleepTime = 1;

    // 현재 수면 시간을 표시하는 UI
    private Text currentTex;

    private void Start()
    {
        // 같은 오브젝트의 Text 컴포넌트 참조
        currentTex = GetComponent<Text>();
    }

    private void ChangeSleepTime(int amount)
{
    int currentTime = int.Parse(currentText.text);
    currentTime = Mathf.Clamp(currentTime + amount, minSleepTime, maxSleepTime);  // 최대,최소 시간을 넘지 않도록 제한
    currentText.text = currentTime.ToString();                                    // 변경된 시간 표시
}

public void UpBtn() => ChangeSleepTime(1);                                        // 수면 시간을 1시간 증가
public void DownBtn() => ChangeSleepTime(-1);                                     // 수면 시간을 1시간 감소
}
