using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TutorialResultManager.Instance != null)
        {
            Debug.Log("튜토리얼 결과: " + TutorialResultManager.Instance.finalResult);

        }
        else
        {
            Debug.LogError("TutorialResultManager.Instance 가 없습니다! (전역 매니저가 씬에 로드되지 않았음)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
