using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : ViewerUI
{
	[SerializeField] private List<Image> DiaryPieceList = new List<Image>();
	[SerializeField] private List<Button> PieceBtnList = new List<Button>();
	[SerializeField] private Button diaryBtn;

	/*private List<bool> DiaryPieceCollected = new List<bool>();
	private int DiaryPieceIndex = -1;*/

	private void Awake()
	{
		//InitDiaryPieceCollected();
	}

	public override  void HideUI()
	{
		HideAllPiece();
		UIManager.Instance.HideViewer(ViewerUIType.Diary);
	}
	
	/*private void InitDiaryPieceCollected()
	{
		for(int i = 0; i < DiaryPieceList.Count; i++)
		{
			DiaryPieceCollected.Add(false);
		}
	}*/

	public void UnlockDiaryPiece(int index)
	{
		//DiaryPieceCollected[index] = true;
		PieceBtnList[index].gameObject.SetActive(true);
		ShowDiaryPice(index);
	}

	public void ShowDiaryPice(int index)
	{
		HideAllPiece();
		//DiaryPieceIndex = index;
		DiaryPieceList[index].gameObject.SetActive(true);
		diaryBtn.gameObject.SetActive(true);
	}

	public void HideAllPiece()
	{
		//DiaryPieceIndex = -1;

		for(int i = 0; i < DiaryPieceList.Count; i++)
		{
			DiaryPieceList[i].gameObject.SetActive(false);
		}

		diaryBtn.gameObject.SetActive(false);
	}

	/*public void MoveOnNextPiece()
	{
		if (DiaryPieceIndex >= DiaryPieceList.Count - 1) return;

		if(DiaryPieceIndex > -1) DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(false);
		DiaryPieceIndex++;

		if (DiaryPieceCollected[DiaryPieceIndex] == true)
			DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(true);
	}*/

	/*public void MoveOnPreviousPiece()
	{
		if (DiaryPieceIndex <= -1) return;

		DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(false);
		DiaryPieceIndex--;

		if (DiaryPieceIndex == -1) return;

		if (DiaryPieceCollected[DiaryPieceIndex] == true)
			DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(true);
	}*/
}
