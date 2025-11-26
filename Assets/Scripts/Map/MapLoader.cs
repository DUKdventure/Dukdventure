using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public GameObject targetGroup;

    // 원하는 시작 색 직접 지정
    public Color32 imageStartColor = new Color32(124, 124, 124, 124);
    public Color32 textStartColor = new Color32(0, 0, 0, 124);

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowAndFade()
    {
        // 이미지 & 텍스트 가져오기
        Image img = targetGroup.GetComponentInChildren<Image>(true);
        TextMeshProUGUI txt = targetGroup.GetComponentInChildren<TextMeshProUGUI>(true);

        if (img == null || txt == null)
        {
            Debug.LogError("MapLoader: 이미지 또는 TMP 텍스트를 찾을 수 없습니다.");
            return;
        }

        // 1) 시작 색상을 직접 지정 (매번 동일하게)
        img.color = imageStartColor;
        txt.color = textStartColor;

        // 2) 그룹 활성화
        targetGroup.SetActive(true);

        // 3) 페이드 아웃 시작
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float duration = 1.2f;
        float time = 0f;

        Image img = targetGroup.GetComponentInChildren<Image>(true);
        TextMeshProUGUI txt = targetGroup.GetComponentInChildren<TextMeshProUGUI>(true);

        // 시작 색상 (지정된 스타트 컬러)
        Color startImg = img.color;
        Color endImg = startImg;
        endImg.a = 0f; // 알파만 0으로

        Color startTxt = txt.color;
        Color endTxt = startTxt;
        endTxt.a = 0f;

        // 서서히 알파 줄이기
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            img.color = Color.Lerp(startImg, endImg, t);
            txt.color = Color.Lerp(startTxt, endTxt, t);

            yield return null;
        }

        // 최종 색상 적용
        img.color = endImg;
        txt.color = endTxt;

        // 비활성화
        targetGroup.SetActive(false);
    }
}
