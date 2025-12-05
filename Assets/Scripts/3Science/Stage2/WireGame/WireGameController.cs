using System.Collections;
using UnityEngine;
using TMPro;

public class WireGameController : MonoBehaviour
{
    public RandomizeChildren rightGroupRandomizer;

    [Header("Wires")]
    public WireEndpoint[] leftWires;     // 왼쪽 드래그 가능한 와이어들
    public WireSocket[] rightSockets;    // 오른쪽 소켓들

    [Header("Pattern")]
    public float showOnTime = 0.6f;
    public float showOffTime = 0.25f;

    WireEndpoint[] sequence;   // 패턴 순서
    int currentIndex = 0;
    bool showingPattern = false;

    [Header("Round / Timer")]
    public int targetRounds = 3;
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;
    public ClearPanel clearPanel;
    public FailPanel failPanel;

    int currentRound = 0;
    float remainTime;
    bool playing = false;
    bool gameEnded = false;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!playing || gameEnded) return;

        // 타이머
        remainTime -= Time.deltaTime;
        if (remainTime <= 0f)
        {
            remainTime = 0f;
            FailGame();
        }

        if (timerText)
            timerText.text = Mathf.CeilToInt(remainTime).ToString();
    }

    public void StartGame()
    {
        currentRound = 0;
        remainTime = timeLimit;
        gameEnded = false;
        playing = true;

        if (clearPanel) clearPanel.HideInstant();
        if (failPanel) failPanel.HideInstant();

        StartNewRound();
    }

    void StartNewRound()
    {
        // 전체 상태 리셋
        foreach (var w in leftWires)
            if (w != null) w.ResetState();

        foreach (var s in rightSockets)
            if (s != null) s.ResetState();

        if (rightGroupRandomizer != null)
            rightGroupRandomizer.Randomize();
        // 새 패턴 생성
        BuildRandomSequence();
        StartCoroutine(ShowPattern());
    }

    void BuildRandomSequence()
    {
        sequence = (WireEndpoint[])leftWires.Clone();

        // Fisher-Yates shuffle
        for (int i = 0; i < sequence.Length; i++)
        {
            int r = Random.Range(i, sequence.Length);
            var tmp = sequence[i];
            sequence[i] = sequence[r];
            sequence[r] = tmp;
        }

        currentIndex = 0;
    }

    IEnumerator ShowPattern()
    {
        showingPattern = true;

        // 먼저 전등 전부 끄기
        foreach (var w in leftWires)
            if (w != null) w.SetPatternLight(false);

        yield return new WaitForSeconds(0.5f);

        // 순서대로 하나씩 켰다가 끔
        foreach (var w in sequence)
        {
            if (w == null) continue;
            w.SetPatternLight(true);
            yield return new WaitForSeconds(showOnTime);
            w.SetPatternLight(false);
            yield return new WaitForSeconds(showOffTime);
        }

        showingPattern = false;
    }

    // ===== 드래그 관련 =====

    public bool CanDrag(WireEndpoint wire)
    {
        if (!playing || gameEnded) return false;
        if (showingPattern) return false;
        if (wire.IsConnected) return false;
        return true;
    }

    public WireSocket FindNearestSocket(Vector3 pos, string wireId, float radius)
    {
        WireSocket best = null;
        float bestDist = radius;

        foreach (var s in rightSockets)
        {
            if (s == null || s.IsConnected) continue;
            if (s.wireId != wireId) continue;   // 같은 색만

            float d = Vector3.Distance(pos, s.snapPoint.position);
            if (d <= bestDist)
            {
                bestDist = d;
                best = s;
            }
        }

        return best;
    }

    public void TryConnect(WireEndpoint wire, WireSocket socket)
    {
        if (!playing || gameEnded) { wire.ResetState(); return; }
        if (showingPattern) { wire.ResetState(); return; }
        if (socket.IsConnected) { wire.ResetState(); return; }

        // 순서 체크
        if (sequence[currentIndex] != wire)
        {
            // 틀린 순서 -> 전체 리셋 + 패턴 다시
            StartCoroutine(HandleWrongOrder());
            return;
        }

        //올바른 순서 + 색상 매치
        wire.AttachToSocket(socket);
        socket.Connect(wire);

        currentIndex++;

        // 모든 와이어 연결 완료 -> 라운드 클리어
        if (currentIndex >= sequence.Length)
        {
            HandleRoundCleared();
        }
    }

    IEnumerator HandleWrongOrder()
    {
        // 잠깐 기다렸다가 (플레이어가 실수 본 다음)
        yield return new WaitForSeconds(0.3f);

        // 전체 리셋 후 패턴 재표시
        foreach (var w in leftWires)
            if (w != null) w.ResetState();

        foreach (var s in rightSockets)
            if (s != null) s.ResetState();

        BuildRandomSequence();
        StartCoroutine(ShowPattern());
    }

    void HandleRoundCleared()
    {
        currentRound++;

        if (currentRound >= targetRounds)
        {
            ClearGame();
        }
        else
        {
            StartNewRound();
        }
    }

    // ===== 클리어 / 실패 =====

    void ClearGame()
    {
        playing = false;
        gameEnded = true;

        if (clearPanel != null)
        {
            clearPanel.ShowClear(
                score: currentRound,              // 성공한 라운드 수
                onRetry: () => StartGame(),
                onNext: () => GameStateManager.Instance?.MiniGameClear(),
                onMenu: () => GameStateManager.Instance?.MiniGameExit()
            );
        }
    }

    void FailGame()
    {
        if (gameEnded) return;

        playing = false;
        gameEnded = true;

        if (failPanel != null)
        {
            failPanel.ShowFail(
                score: currentRound,
                onRetry: () => StartGame(),
                onExit: () => GameStateManager.Instance?.MiniGameExit()
            );
        }
    }
}
