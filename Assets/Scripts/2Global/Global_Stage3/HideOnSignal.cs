using UnityEngine;

public class HideOnSignal : MonoBehaviour
{
    public int miniGameID;  // 이 오브젝트가 어떤 미니게임과 연결되는지 ID 설정

    private void Start()
    {
        if (G_GameStateManager.Instance.clearedMiniGames.ContainsKey(miniGameID) &&
            G_GameStateManager.Instance.clearedMiniGames[miniGameID] == true)
        {
            gameObject.SetActive(false);
        }
    }
}
