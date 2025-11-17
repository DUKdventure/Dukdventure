using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    //미니게임에서 돌아올 씬/위치 정보
    public string returnSceneName;

    //성공/나가기 위치 저장
    Vector2 successReturnPosition;
    Vector2 exitReturnPosition;

    //다음 씬이 로드될 때 사용할 스폰 위치
    public Vector2 nextSpawnPosition;
    public bool hasPendingSpawn; 

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //비커존에서 미니게임 들어가기 직전에 호출
    public void SetReturnPoints(string sceneName, Vector2 successPos, Vector2 exitPos)
    {
        returnSceneName = sceneName;
        successReturnPosition = successPos;
        exitReturnPosition = exitPos;
    }

    //미니게임 클리어 시 호출
    public void MiniGameClear()
    {
        if (string.IsNullOrEmpty(returnSceneName)) return;

        nextSpawnPosition = successReturnPosition;
        hasPendingSpawn = true;

        SceneManager.LoadScene(returnSceneName);
    }
    
    //미니게임 "나가기" 눌렀을 때 (실패 후 포기)
    public void MiniGameExit()
    {
        if(!string.IsNullOrEmpty(returnSceneName))
        {
            if (string.IsNullOrEmpty(returnSceneName)) return;

            nextSpawnPosition = exitReturnPosition;
            hasPendingSpawn = true;

            SceneManager.LoadScene(returnSceneName);
        }
    }
}
