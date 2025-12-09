using UnityEngine;

public class BeakerGameManager : MonoBehaviour
{
    public static BeakerGameManager Instance { get; private set; }

    [Header("Rounds")]
    public GameObject[] roundGroups;         // 0~3 인덱스 → Round1~4_Group

    [Header("SpawnPoints")]
    public Transform[] roundSpawnPoints;     // 0~3 인덱스 → Round1~4_Spawn

    [Header("Player")]
    public Transform player;                 // Player Transform
    public PlayerSafeCheck playerSafeCheck;  // Player에 붙어 있는 SafeCheck

    [Header("ClearPanel")]
    public ClearPanel clearPanel;            // 마지막 라운드 클리어 시 띄울 패널
    public FailPanel failPanel;

    int currentRound = 0;                    // 0 = 1라운드
    int totalRounds = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        totalRounds = roundGroups != null ? roundGroups.Length : 0;
    }

    void Start()
    {
        // 시작은 항상 0번 라운드부터
        SetRound(0);
    }

    void Update()
    {
        // ESC 누르면 실패 패널
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowFailByEsc();
        }
    }

    /// <summary>
    /// 특정 라운드로 전환 (0-based)
    /// </summary>
    public void SetRound(int index)
    {
        failPanel.gameObject.SetActive(false);

        if (totalRounds == 0) return;

        currentRound = Mathf.Clamp(index, 0, totalRounds - 1);

        // 라운드 그룹 On/Off
        for (int i = 0; i < roundGroups.Length; i++)
        {
            if (roundGroups[i] != null)
                roundGroups[i].SetActive(i == currentRound);
        }

        // 스폰 포인트 위치로 플레이어 이동 + SafeCheck 갱신
        if (roundSpawnPoints != null && currentRound < roundSpawnPoints.Length)
        {
            Transform spawn = roundSpawnPoints[currentRound];

            if (spawn != null)
            {
                if (player != null)
                    player.position = spawn.position;

                if (playerSafeCheck != null)
                    playerSafeCheck.SetSpawnPoint(spawn, teleportPlayer: false);
            }
        }
    }

    /// <summary>
    /// 클리어 포인트에 닿았을 때 호출되는 함수
    /// </summary>
    public void OnReachClearPoint(int roundIndex)
    {
        // 혹시 다른 라운드용 클리어 포인트가 남아있을 수 있으니 방어코드
        if (roundIndex != currentRound) return;

        // 마지막 라운드가 아니면 다음 라운드로
        if (currentRound < totalRounds - 1)
        {
            SetRound(currentRound + 1);
        }
        else
        {
            // === 마지막 라운드 클리어 ===
            if (clearPanel != null)
            {
                // 점수 시스템 없으면 0 넣어도 됨
                clearPanel.ShowClear(
                    score: 4,
                    onRetry: () =>
                    {
                        // 다시 1라운드부터 시작
                        SetRound(0);
                    },
                    onNext: () =>
                    {
                        if (GameStateManager.Instance != null)
                        {
                            GameStateManager.Instance.MiniGameClear();
                        }
                    },
                    onMenu: () =>
                    {
                        // 메인 메뉴로 나가기 등
                        // SceneManager.LoadScene("MainScene"); 이런 거 넣으면 됨
                    });
            }
            else
            {
                Debug.Log("마지막 라운드 클리어! (clearPanel이 지정되어 있지 않음)");
            }
        }
    }

    void ShowFailByEsc()
    {
        if (failPanel == null)
        {
            Debug.LogWarning("FailPanel 이 연결되어 있지 않습니다.");
            return;
        }

        // 점프/이동 잠깐 막고 싶으면 여기서 플레이어 컨트롤 끄기
        var player = FindAnyObjectByType<PlayerJumpTopDown>();
        if (player != null)
        {
            player.enabled = false;
        }

        failPanel.ShowFail(
            score: currentRound,

            // 다시 하기 → 이 미니게임 리셋
            onRetry: () =>
            {
                // 컨트롤 다시 켜기
                if (player != null)
                    player.enabled = true;

                SetRound(0);
            },

            // 나가기 → GameStateManager 통해 원래 씬으로
            onExit: () =>
            {
                if (GameStateManager.Instance != null)
                {
                    GameStateManager.Instance.MiniGameExit();
                }
            }
        );
    }
}
