using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameZone : MonoBehaviour
{
    [Header("MiniGame Scene")]
    public string miniGameSceneName;

    [Header("After Clear")]
    public Transform successReturnPoint;

    [Header("After Exit")]
    public Transform exitReturnPoint;

    bool isTriggered = false;

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered )
            return;

        if (!other.CompareTag("Player")) 
            return;

        isTriggered = true;

        //1. 돌아올 위치/씬 저장
        var gsm = GameStateManager.Instance;

        if( gsm != null)
        {
            gsm.SetReturnPoints(
                SceneManager.GetActiveScene().name,
                successReturnPoint.position,
                exitReturnPoint.position
            );
        }

        //2. 미니게임 씬 로드
        SceneManager.LoadScene(miniGameSceneName);
    }

}
