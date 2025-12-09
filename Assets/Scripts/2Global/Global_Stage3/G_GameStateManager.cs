using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class G_GameStateManager : MonoBehaviour
{
    public static G_GameStateManager Instance { get; private set; }

    // 어떤 미니게임이 클리어되었는지 저장하는 딕셔너리
    public Dictionary<int, bool> clearedMiniGames = new Dictionary<int, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 미니게임 클리어 신호
    public void MiniGameClear(int id)
    {
        clearedMiniGames[id] = true;
    }

    // 씬 로드될 때 클리어된 오브젝트 삭제
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 아무 것도 할 필요 없음
        // 각 오브젝트가 스스로 “내 ID가 클리어됐는지” 체크함
    }
}
