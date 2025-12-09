using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public static HeartManager Instance;

    public GameObject GameOverPanel;
    public GameObject[] heartIcons;   // UI 하트 오브젝트 배열
    private int currentIndex;         // 현재 꺼질 하트의 인덱스

    private void Awake()
    {
        Instance = this;
    }

    public void RemoveHeart()
    {
        if (currentIndex < heartIcons.Length)
        {
            heartIcons[currentIndex].SetActive(false);
            currentIndex++;

            Debug.Log($"하트 제거됨 ({currentIndex}/{heartIcons.Length})");

            if (currentIndex >= heartIcons.Length)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("게임 오버!");
        GameOverPanel.SetActive(true);
    }
}
