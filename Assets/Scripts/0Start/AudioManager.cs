using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [Header("Mixer")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] string bgmParam = "BGM_Volume";
    [SerializeField] string sfxParam = "SFX_Volume";

    [Header("Audio Sources")]
    [SerializeField] AudioSource bgmSource;   // BGM 전용
    [SerializeField] AudioSource sfxSource;   // 공용 SFX 전용

    const string KEY_BGM = "vol.bgm";
    const string KEY_SFX = "vol.sfx";

    void Awake()
    {
        // 싱글턴 & 씬 유지
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

        // 저장된 볼륨 불러오기
        float bgm = PlayerPrefs.GetFloat(KEY_BGM, 0.8f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 0.8f);
        SetBgm01(bgm, save: false);
        SetSfx01(sfx, save: false);
    }

    // ========= BGM =========

    // 특정 BGM 클립 재생
    public void PlayBgm(AudioClip clip, bool loop = true)
    {
        if (clip == null || bgmSource == null) return;

        // 같은 클립이 이미 재생 중이면 패스
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBgm()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }

    public void PauseBgm()
    {
        if (bgmSource == null) return;
        bgmSource.Pause();
    }

    public void ResumeBgm()
    {
        if (bgmSource == null) return;
        bgmSource.UnPause();
    }

    public void ToggleBgm()
    {
        if (bgmSource == null) return;

        if (bgmSource.isPlaying)
            PauseBgm();
        else
            ResumeBgm();
    }

    // ========= SFX =========

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // ========= 볼륨 (0~1) =========

    public void SetBgm01(float v, bool save = true)
    {
        if (mixer == null) return;

        mixer.SetFloat(bgmParam, Linear01ToDb(v));

        if (save)
            PlayerPrefs.SetFloat(KEY_BGM, v);
    }

    public void SetSfx01(float v, bool save = true)
    {
        if (mixer == null) return;

        mixer.SetFloat(sfxParam, Linear01ToDb(v));

        if (save)
            PlayerPrefs.SetFloat(KEY_SFX, v);
    }

    // ========= 유틸 =========

    float Linear01ToDb(float v)
    {
        // 0~1 → dB (0은 -∞ 대신 -80 정도로)
        if (v <= 0.0001f)
            return -80f;
        return Mathf.Log10(v) * 20f;
    }

    public float GetSavedBgm01()
    {
        return PlayerPrefs.GetFloat(KEY_BGM, 0.8f);
    }

    public float GetSavedSfx01()
    {
        return PlayerPrefs.GetFloat(KEY_SFX, 0.8f);
    }
}
