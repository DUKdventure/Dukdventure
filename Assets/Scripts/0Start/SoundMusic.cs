using UnityEngine;

public class SoundMusic : MonoBehaviour
{
    GameObject backgroundMusic;
    AudioSource backMusic;
    void Awake()
    {
        backgroundMusic = GameObject.Find("BackGroundMusic");
        backMusic = backgroundMusic.GetComponent<AudioSource>();
        if (backMusic.isPlaying)
            return;
        else
        {
            backMusic.Play();
            DontDestroyOnLoad(backMusic);
        }
    }

    public void BackGroundMusicButton()
    {
        backgroundMusic = GameObject.Find("BackGroundMusic");
        backMusic = backgroundMusic.GetComponent<AudioSource>();

        if(backMusic.isPlaying) 
            backMusic.Pause();
        else 
            backMusic.Play();
    }
}
