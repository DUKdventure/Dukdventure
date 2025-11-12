using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReagentQueueGame : MonoBehaviour
{
    [Header("Data Pool")]
    public List<ReagentData> pool;
    public int totalCount = 30;     // 전체 문제 수
    public int preloadVisible = 4;  // 최초 보이는 장 수(QueueView.visibleCount와 맞추기)

    [Header("UI")]
    public ReagentQueueView queueView;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI leftText;
    public Image[] colorButtons; // 0=Red,1=Green,2=Blue (하이라이트용)

    [Header("Rule")]
    public int scoreCorrect = 100;
    public int scoreWrong = -50;

    Queue<ReagentData> deck;
    int left;
    int score;
    bool locked;

    void Start()
    {
        BuildDeck();
        SetupStart();
        UpdateHUD();
    }

    void BuildDeck()
    {
        // 간단 랜덤 덱
        deck = new Queue<ReagentData>();
        for (int i = 0; i < totalCount; i++)
        {
            deck.Enqueue(pool[Random.Range(0, pool.Count)]);
        }
        left = totalCount;
    }

    void SetupStart()
    {
        queueView.ClearAll();
        int vis = Mathf.Min(preloadVisible, left);
        for (int i = 0; i < vis; i++)
        {
            var d = deck.Dequeue();
            queueView.PushBack(d, instant: true);
        }
        // 남은 개수 갱신
        left = totalCount - vis;
    }

    void Update()
    {
        if (locked) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Choose(0);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Choose(1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Choose(2);
    }

    public void Choose(int index)
    {
        if (locked) return;
        var front = queueView.Front;
        if (!front) { EndGame(); return; }

        Highlight(index);

        var chosen = (DyeColor)index;
        var trueColor = front.nameText ? chosen /*dummy*/ : chosen; // nameText 미사용 시에도 안전
        // 실제 정답은 카드에 저장된 데이터 필요 → 아이콘에 보관하지 않으므로 별도 참조가 필요
        // 방법: 카드에 현재 데이터의 targetColor를 들고 있게 하자.
        // 간단히 캐시:
        var cardDataHolder = front.GetComponent<ReagentCardDataHolder>();
        if (!cardDataHolder) { cardDataHolder = front.gameObject.AddComponent<ReagentCardDataHolder>(); cardDataHolder.target = DyeColor.Red; } // 안전장치
        bool ok = (chosen == cardDataHolder.target);

        locked = true;
        if (ok) score += scoreCorrect; else score += scoreWrong;
        UpdateHUD();

        if (ok)
        {
            // 앞 카드 제거 & 앞으로 이동 애니
            queueView.RemoveFrontAndShiftForward(() =>
            {
                // 뒤에 새 카드 추가(남아 있으면)
                if (deck.Count > 0)
                {
                    var d = deck.Dequeue();
                    queueView.PushBack(d);
                    left--;
                }
                locked = false;
                Highlight(-1);
                if (queueView.Count == 0 && deck.Count == 0) EndGame();
            });
        }
        else
        {
            // 오답 피드백: 앞 카드 살짝 흔들기
            StartCoroutine(Shake(front.RT, 0.15f, 8f, () =>
            {
                locked = false;
                Highlight(-1);
            }));
        }
    }

    void UpdateHUD()
    {
        if (scoreText) scoreText.text = $"Score {score}";
        if (leftText) leftText.text = $"Left {left + queueView.Count}";
    }

    void EndGame()
    {
        Debug.Log($"GAME OVER | Score={score}");
        // 결과 패널 띄우기 등
        enabled = false;
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

    System.Collections.IEnumerator Shake(RectTransform rt, float dur, float angle, System.Action done)
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

public class ReagentCardDataHolder : MonoBehaviour
{
    public DyeColor target;
}
