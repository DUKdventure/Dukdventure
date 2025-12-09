using UnityEngine;
using UnityEngine.UI;

public class AudioSliderBinder : MonoBehaviour
{
    [Header("슬라이더 레퍼런스")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (AudioManager.I == null) return;

        // BGM 슬라이더
        if (bgmSlider != null)
        {
            bgmSlider.minValue = 0f;
            bgmSlider.maxValue = 1f;

            // 저장된 값으로 초기값 설정
            bgmSlider.value = AudioManager.I.GetSavedBgm01();

            // 값이 바뀔 때마다 AudioManager에 전달
            bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        }

        // SFX 슬라이더
        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;

            sfxSlider.value = AudioManager.I.GetSavedSfx01();

            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        }
    }

    void OnDestroy()
    {
        if (bgmSlider != null)
            bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
    }

    void OnBgmSliderChanged(float v)
    {
        if (AudioManager.I == null) return;
        AudioManager.I.SetBgm01(v);
    }

    void OnSfxSliderChanged(float v)
    {
        if (AudioManager.I == null) return;
        AudioManager.I.SetSfx01(v);
    }
}
