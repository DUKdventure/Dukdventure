using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [Header("UI")]
    public Image targetImage;          // 버튼에 붙은 Image
    public Sprite soundOnSprite;       // 소리 켜짐 이미지
    public Sprite soundOffSprite;      // 음소거 이미지

    bool isMuted = false;

    // 이전 볼륨 기억용
    float prevBgm = 0.8f;
    float prevSfx = 0.8f;

    void Start()
    {
        if (AudioManager.I == null)
        {
            Debug.LogWarning("AudioManager가 없음");
            return;
        }

        // 현재 저장된 볼륨 기준으로 시작 상태 판단
        float currentBgm = AudioManager.I.GetSavedBgm01();
        float currentSfx = AudioManager.I.GetSavedSfx01();

        // 둘 다 거의 0이면 음소거 상태라고 판단
        isMuted = (currentBgm <= 0.0001f && currentSfx <= 0.0001f);

        // 이전 볼륨값은 일단 현재 값으로 저장
        prevBgm = currentBgm;
        prevSfx = currentSfx;

        UpdateIcon();
    }

    /// <summary>
    /// 버튼 OnClick에 연결할 함수
    /// </summary>
    public void OnClickMuteToggle()
    {
        if (AudioManager.I == null) return;

        if (!isMuted)
        {
            // 현재 볼륨을 저장해두고
            prevBgm = AudioManager.I.GetSavedBgm01();
            prevSfx = AudioManager.I.GetSavedSfx01();

            // 0으로 줄이기 (음소거)
            AudioManager.I.SetBgm01(0f);
            AudioManager.I.SetSfx01(0f);

            isMuted = true;
        }
        else
        {
            // 저장해둔 값으로 복구 (최소 0~1 사이로 클램프)
            prevBgm = Mathf.Clamp01(prevBgm);
            prevSfx = Mathf.Clamp01(prevSfx);

            AudioManager.I.SetBgm01(prevBgm);
            AudioManager.I.SetSfx01(prevSfx);

            isMuted = false;
        }

        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (targetImage == null) return;

        if (isMuted)
        {
            if (soundOffSprite != null)
                targetImage.sprite = soundOffSprite;
        }
        else
        {
            if (soundOnSprite != null)
                targetImage.sprite = soundOnSprite;
        }
    }
}
