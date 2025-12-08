using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fadeImage;
    public float fadeDuration = 0.8f;

    private void Awake()
    {
        Instance = this;
    }

    // 색상과 씬을 넘겨받아 페이드 실행
    public void PlayFade(Color fadeColor, string sceneName)
    {
        StartCoroutine(FadeRoutine(fadeColor, sceneName));
    }

    private IEnumerator FadeRoutine(Color fadeColor, string sceneName)
    {
        // 🔥 (추가) 페이드 이미지 활성화
        fadeImage.gameObject.SetActive(true);

        // 초기 컬러(alpha = 0)
        fadeColor.a = 0f;
        fadeImage.color = fadeColor;

        float t = 0;

        // 1) 페이드 인 (점점 진해짐)
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeColor.a = a;
            fadeImage.color = fadeColor;
            yield return null;
        }

        // 2) 1초 유지
        yield return new WaitForSeconds(1f);


        // 4) 씬 이동
        SceneManager.LoadScene(sceneName);
    }
}
