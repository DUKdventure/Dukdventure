using UnityEngine;

public class GlobalUIRoot : MonoBehaviour
{
    public static GlobalUIRoot Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame called");

        // 에디터에서는 종료되지 않고 로그만 남김
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // 빌드된 게임에서 종료
    Application.Quit();
#endif
    }
}
