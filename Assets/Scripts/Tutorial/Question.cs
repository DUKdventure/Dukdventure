using UnityEngine;
using System;

public class Question : MonoBehaviour
{
    public QuestionManager manager;
    public GameObject nextQuestion;
    public double[] optionScores;

    public GameObject loading;

    public void OnAnswerSelected(int optionIndex)
    {
        double score = 0;
        if (optionIndex < optionScores.Length)
            score = optionScores[optionIndex];

        manager.AddScore(score);

        gameObject.SetActive(false);

        if (nextQuestion != null)
        {
            nextQuestion.SetActive(true);
        }
        else
        {
            Debug.Log("모든 질문 완료!");
            manager.StartCoroutine(manager.Loading());
        }
    }
}
