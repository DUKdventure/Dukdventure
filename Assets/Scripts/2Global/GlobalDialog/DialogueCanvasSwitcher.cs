using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialoguePanelSwitcher : MonoBehaviour
{
    public GameObject panelA;
    public GameObject panelB;

    [Header("옵션")]
    public bool useFade = false;       // ← 체크박스
    public Image fadeImage;            // ← 페이드용 이미지(검정 화면)
    public float fadeDuration = 1f;    // 페이드 속도

    void Update()
    {
        if (panelA == null || panelB == null) return;

        // A가 꺼지고 B가 꺼져있다면 전환 실행
        if (!panelA.activeInHierarchy && !panelB.activeSelf)
        {
            if (useFade)
            {
                StartCoroutine(FadeAndSwitch());
            }
            else
            {
                Debug.Log("[PanelSwitch] 즉시 전환 실행");
                panelB.SetActive(true);
            }
        }
    }

    IEnumerator FadeAndSwitch()
    {
        Debug.Log("[PanelSwitch] 페이드아웃 실행");

        // FadeImage 켜기
        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        // 알파값 올리기
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // 페이드 완료 후 PanelB 켜기
        panelB.SetActive(true);

        Debug.Log("[PanelSwitch] 페이드 완료 → PanelB 활성화됨");
    }
}
