using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialoguePanelSwitcher : MonoBehaviour
{
    public GameObject panelA;
    public GameObject panelB;

    [Header("옵션")]
    public bool useFade = false;       // 체크박스
    public Image fadeImage;            // 페이드용 이미지(검정 화면)
    public float fadeDuration = 1f;    // 페이드 속도
    public float waitDuration = 2f;    // 어두운 화면에서 대기 시간

    bool isSwitching = false;

    void Update()
    {
        if (panelA == null || panelB == null) return;
        if (isSwitching) return;

        // A가 꺼지고 B가 꺼져있다면 전환 실행
        if (!panelA.activeInHierarchy && !panelB.activeSelf)
        {
            isSwitching = true;

            if (useFade)
            {
                StartCoroutine(FadeTransition());
            }
            else
            {
                Debug.Log("[PanelSwitch] 즉시 전환 실행");
                panelB.SetActive(true);
            }
        }
    }

    IEnumerator FadeTransition()
    {
        Debug.Log("[PanelSwitch] 페이드아웃 시작");

        // FadeImage 활성화
        fadeImage.gameObject.SetActive(true);

        // 초기 색상
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        // 🔥 1) 페이드아웃 (0 → 1)
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // 🔥 2) 어두운 화면 유지
        Debug.Log("[PanelSwitch] 어두운 화면 유지 중...");
        yield return new WaitForSeconds(waitDuration);

        // 🔥 3) Panel B 활성화
        panelB.SetActive(true);
        Debug.Log("[PanelSwitch] Panel B 활성화됨");

        // 🔥 4) 페이드인 (1 → 0)
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // 페이드화면 꺼짐
        fadeImage.gameObject.SetActive(false);
        Debug.Log("[PanelSwitch] 페이드 전환 완료");
    }
}
