using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 전체 게임의 메인 매니저.
 * - 시약 생성 및 순서 관리
 * - 플레이어 입력 처리 (좌/상/우)
 * - 시간 제한, 점수 계산
 *
 * 주요 기능:
 *  - Start() : 초기화, 큐 세팅
 *  - Update() : 타이머 감소 + 입력 감지
 *  - Choose(int index) : 플레이어 선택 처리
 *  - EndGame() : 시간 종료 시 게임 오버 처리
 */
public class ReagentGame : MonoBehaviour
{
    [Header("Data Pool")]
    public List<ReagentData> pool;
    public int preloadVisible = 5;  //최초 보이는 장 수(QueueView.visibleCount와 맞추기)

    [Header("Episode Intro")]
    public GameObject episodePanel;   //시작 설명 패널
    public float episodeDelay = 3f;   //몇 초 동안 보여줄지

    [Header("UI")]
    public ReagentView queueView;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI goalText;
    public Image[] colorButtons; //0=Red, 1=Green, 2=Blue

    [Header("Rule")]
    public float timeLimit = 60f;
    public int scoreCorrect = 100;
    public int scoreWrong = -50;
    public int targetScore = 1500;

    [Header("Game Over")]
    public GameOverPanel gameOverPanel;
    public string nextSceneName = "";
    public string clearTitle = "STAGE CLEAR!";
    public string failTitle = "TIME UP";

    bool gameStarted = false;
    float timeLeft;
    int score;
    bool locked;
    bool gameEnded;

    void Start()
    {
        if (gameOverPanel) gameOverPanel.HideInstant();

        //에피소드 패널이 있으면 먼저 보여주고 잠시 후 게임 시작
        if (episodePanel != null)
        {
            StartCoroutine(EpisodeIntroRoutine());
        }
        else
        {
            //패널이 없다면 바로 게임 시작
            StartNewRun();
            gameStarted = true;
        }
    }

    IEnumerator EpisodeIntroRoutine()
    {
        gameStarted = false;

        episodePanel.SetActive(true);
        CanvasGroup cg = episodePanel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = episodePanel.AddComponent<CanvasGroup>();

        cg.alpha = 1f;

        yield return new WaitForSecondsRealtime(episodeDelay);

        //페이드 아웃
        float fadeTime = 1f; //페이드 시간
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }
        cg.alpha = 0f;

        episodePanel.SetActive(false);

        // 게임 시작
        StartNewRun();
        gameStarted = true;
    }

    void StartNewRun()
    {
        gameEnded = false;
        locked = false;
        score = 0;
        timeLeft = timeLimit;

        queueView.ClearAll();
        for (int i = 0; i < preloadVisible; i++)
        {
            var d = pool[Random.Range(0, pool.Count)];
            queueView.PushBack(d, instant: true);
        }
        queueView.RelayoutForward();
        UpdateHUD();
    }


    void Update()
    {
        if (!gameStarted) return;   //에피소드 패널 띄워진 동안에는 게임 로직 안 돌림
        if (gameEnded) return;

        //타이머
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;

                if (score >= targetScore) TriggerClear();
                else EndGame_Fail();
            }
            UpdateHUD();
        }

        if (locked || timeLeft <= 0f) return;
       
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) Choose(0);
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) Choose(1);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) Choose(2);
    }

    public void Choose(int index)
    {
        if (locked || gameEnded ||  timeLeft <= 0f) return;

        var front = queueView.Front;
        if (!front) return; 

        Highlight(index);

        var holder = front.GetComponent<ReagentCardDataHolder>();
        var chosen = (DyeColor)index;
        bool ok = holder && holder.target == chosen;

        locked = true;
        score += ok ? scoreCorrect : scoreWrong;
        UpdateHUD();

        if (score >= targetScore)
        {
            Highlight(-1);
            TriggerClear();
            return;
        }

        if (ok)
        {
            //앞 시약 제거 & 앞으로 이동 애니
            queueView.RemoveFrontAndShiftForward(() =>
            {
                //뒤에 새 시약 추가(남아 있으면)
                var nd = pool[Random.Range(0, pool.Count)];
                queueView.PushBack(nd);
                locked = false;
                Highlight(-1);
            });
        }
        else
        {
            //오답 피드백: 앞 시약 살짝 흔들기
            StartCoroutine(Shake(front.RT, 0.15f, 8f, () =>
            {
                locked = false;
                Highlight(-1);
            }));
        }
    }

    void TriggerClear()
    {
        if (gameEnded) return;
        gameEnded = true;
        locked = true;

        if (gameOverPanel)
        {
            gameOverPanel.ShowClear(
                clearTitle, score,
                onNext: () => {
                    if (!string.IsNullOrEmpty(nextSceneName) && SceneLoader.Instance != null)
                        SceneLoader.Instance.LoadScene(nextSceneName);
                },
                onQuit: null //필요하면 메뉴 씬 로드 콜백 넣기
            );
        }
        else
        {
            //패널이 없다면 즉시 로드
            if (!string.IsNullOrEmpty(nextSceneName) && SceneLoader.Instance != null)
                SceneLoader.Instance.LoadScene(nextSceneName);
        }
    }


    void UpdateHUD()
    {
        if (scoreText) scoreText.text = $"{score}";
        if (timeText) timeText.text = $"{Mathf.CeilToInt(timeLeft)}";
        if (goalText) goalText.text = $"{targetScore}";
    }

    void EndGame_Fail()
    {
        if (gameEnded) return;
        gameEnded = true;
        locked = true;

        if (gameOverPanel)
        {
            gameOverPanel.ShowFail(
                failTitle, score,
                onRetry: () => { RestartGame(); },
                onQuit: null // 필요 시 메뉴로
            );
        }
        else
        {
            Debug.Log($"TIME UP | Score={score}");
        }
    }

    public void RestartGame()
    {
        // 패널 숨기기
        if (gameOverPanel) gameOverPanel.HideInstant();

        // 초기화 & 재시작
        StartNewRun();
    }

    public void QuitToMenu(string sceneName = "")
    {
        if (!string.IsNullOrEmpty(sceneName) && SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(sceneName);
        else
            Debug.Log("QuitToMenu: 씬 이름이 비었거나 SceneLoader가 없습니다.");
    }

    void Highlight(int index)
    {
        if (colorButtons == null) return;
        for (int i = 0; i < colorButtons.Length; i++)
        {
            var img = colorButtons[i];
            if (!img) continue;
            img.transform.localScale = (i == index) ? Vector3.one * 1.06f : Vector3.one;
            var c = img.color;
            img.color = new Color(c.r, c.g, c.b, (i == index) ? 1f : 0.85f);
        }
    }

    IEnumerator Shake(RectTransform rt, float dur, float angle, System.Action done)
    {
        float t = 0; var start = rt.localEulerAngles;
        while (t < dur)
        {
            t += Time.deltaTime;
            float s = Mathf.Sin(t * 40f) * angle * (1f - t / dur);
            rt.localEulerAngles = new Vector3(0, 0, s);
            yield return null;
        }
        rt.localEulerAngles = start;
        done?.Invoke();
    }

}
