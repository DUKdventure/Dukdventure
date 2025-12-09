using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialoguePanelSwitcher : MonoBehaviour
{
    public GameObject panelA;
    public GameObject panelB;

    [Header("Fade Options")]
    public bool fadeOut = true;    // 화면을 어둡게
    public bool fadeIn = true;     // 화면을 밝게
    public Image fadeImage;
    public float fadeDuration = 1f;

    private bool isSwitching = false;

    void Update()
    {
        if (isSwitching) return;
        if (panelA == null || panelB == null) return;

        if (!panelA.activeInHierarchy && !panelB.activeSelf)
        {
            StartCoroutine(FadeProcess());
        }
    }


    IEnumerator FadeProcess()
    {
        isSwitching = true;

        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;

        // ---------------------------
        // 1) Fade Out (0 → 1)
        // ---------------------------
        if (fadeOut)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        // 패널 교체
        panelB.SetActive(true);

        // ---------------------------
        // 2) Fade In (1 → 0)
        // ---------------------------
        if (fadeIn)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        // 끝나면 페이드 이미지 숨김
        fadeImage.gameObject.SetActive(false);
        isSwitching = false;
    }
}
