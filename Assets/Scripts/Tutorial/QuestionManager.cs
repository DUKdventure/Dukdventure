using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    private double totalScore = 0;

    [Header("결과 오브젝트들")]
    public GameObject result_Global;     
    public GameObject result_Science;    
    public GameObject result_Pharmacy;   
    public GameObject result_Art;       
    public GameObject result_Future;     
    public GameObject result_Reject;

    public void AddScore(double score)
    {
        totalScore += score;
        Debug.Log($"현재 누적 점수: {totalScore}");
    }

    public void ShowResult()
    {
        result_Global.SetActive(false);
        result_Science.SetActive(false);
        result_Pharmacy.SetActive(false);
        result_Art.SetActive(false);
        result_Future.SetActive(false);
        result_Reject.SetActive(false);

        if (totalScore >= 9.5)
            result_Pharmacy.SetActive(true);     
        else if (totalScore >= 8.5)
            result_Science.SetActive(true);      
        else if (totalScore >= 7.0)
            result_Future.SetActive(true);         
        else if (totalScore >= 5.5)
            result_Global.SetActive(true);       
        else if (totalScore >= 3.0)
            result_Art.SetActive(true);
        else
            result_Reject.SetActive(true);

        Debug.Log($"최종 점수: {totalScore}, 결과창 표시 완료");
    }

    public double GetTotalScore()
    {
        return totalScore;
    }
}
