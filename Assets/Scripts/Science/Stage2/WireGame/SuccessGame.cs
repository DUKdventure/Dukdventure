using UnityEngine;

public class SuccessGame : MonoBehaviour
{
    static public SuccessGame Instance { get; private set; }

    public int switchCount;
    public GameObject winPanel;
    int onCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void SwitchChange(int points)
    {
        onCount += points;
        if(onCount == switchCount)
        {
            winPanel.SetActive(true);
        }
    }

    public void OnClickMapButton()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.MiniGameClear();
        }
    }

    //실패 후 그냥 나가기 버튼도 만들 거면
    public void OnClickExitButton()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.MiniGameExit();
        }
    }
}
