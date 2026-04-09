using UnityEngine;

public class DiaryPieceButton : MonoBehaviour
{
    [SerializeField] private int PieceIndex;

    public void ShowDiaryPieceUI()
    {
        UIManager.Instance.GetDiaryUI().ShowDiaryPice(PieceIndex);
    }
}
