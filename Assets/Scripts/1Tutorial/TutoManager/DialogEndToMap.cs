using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogEndToNextScene : MonoBehaviour
{
    public GameObject dialogPanel;  // DialogPanel 전체 오브젝트
    public string sceneOnNormal = "Map";        // 정상 결과 시 이동할 씬
    public string sceneOnReject = "RejectScene"; // Reject 결과 시 이동할 씬

    private bool moved = false;

    void Update()
    {
        // 대사 중에는 panel이 켜져있음. 끝나면 꺼짐.
        if (!moved && !dialogPanel.activeSelf)
        {
            moved = true;

            // 결과 가져오기
            string result = QuestionManager.finalResult;

            if (result == "Reject")
            {
                SceneManager.LoadScene(sceneOnReject);
            }
            else
            {
                SceneManager.LoadScene(sceneOnNormal);
            }
        }
    }
}
