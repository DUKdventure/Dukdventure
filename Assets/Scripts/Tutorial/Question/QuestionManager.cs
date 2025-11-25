using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    private double totalScore = 0;
    public VideoPlayer videoPlayer;

    public GameObject resultRoot;   // 전체 결과 패널 (배경 + 이미지 포함)

    public Image resultImage;       // Result 안의 이미지
    public GameObject dialogObject; // Dialog.cs가 붙어 있는 오브젝트
    public Dialog dialogScript;  
    public Sprite sprite_Global;     
    public Sprite sprite_Science;    
    public Sprite sprite_Pharmacy;   
    public Sprite sprite_Art;       
    public Sprite sprite_Future;
    public Sprite sprite_Reject;

    public DialogData data_Global;
    public DialogData data_Science;
    public DialogData data_Pharmacy;
    public DialogData data_Art;
    public DialogData data_Future;
    public DialogData data_Reject;
    
    public GameObject loading;

    public void AddScore(double score)
    {
        totalScore += score;
        Debug.Log($"현재 누적 점수: {totalScore}");
    }

    public IEnumerator Loading() { 
        loading.SetActive(true);

    float duration = 1.5f;
    yield return new WaitForSeconds(duration);

    loading.SetActive(false);
    ShowResult();
 }

    public void ShowResult()
{ resultRoot.SetActive(true);

    // 2) 결과에 따라 이미지 설정 + DialogData 설정
    if (totalScore >= 9.5)
    {
        resultImage.sprite = sprite_Pharmacy;
        dialogScript.dialogData = data_Pharmacy;
    }
    else if (totalScore >= 8.5)
    {
        resultImage.sprite = sprite_Science;
        dialogScript.dialogData = data_Science;
    }
    else if (totalScore >= 7.0)
    {
        resultImage.sprite = sprite_Future;
        dialogScript.dialogData = data_Future;
    }
    else if (totalScore >= 5.5)
    {
        resultImage.sprite = sprite_Global;
        dialogScript.dialogData = data_Global;
    }
    else if (totalScore >= 3.0)
    {
        resultImage.sprite = sprite_Art;
        dialogScript.dialogData = data_Art;
    }
    else
    {
        resultImage.sprite = sprite_Reject;
        dialogScript.dialogData = data_Reject;
    }

    // ★★★ 3) Dialog 오브젝트 강제 리셋 (핵심)
    dialogObject.SetActive(false);
    dialogObject.SetActive(true);
}

}
