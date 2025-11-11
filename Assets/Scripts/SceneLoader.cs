using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Loading")]
    public GameObject loadingScreen;
    public VideoPlayer loadingVideo;
    public float minLoadingSeconds = 3f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingScreen) loadingScreen.SetActive(true);

        if (loadingVideo)
        {
            loadingVideo.isLooping = true;
            loadingVideo.Prepare();
            while (!loadingVideo.isPrepared)
                yield return null;
            loadingVideo.Play();
        }

        float startTime = Time.unscaledTime;
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        // 최소 시간 대기
        float remain = (startTime + minLoadingSeconds) - Time.unscaledTime;
        if (remain > 0) yield return new WaitForSecondsRealtime(remain);

        op.allowSceneActivation = true;
    }
}
