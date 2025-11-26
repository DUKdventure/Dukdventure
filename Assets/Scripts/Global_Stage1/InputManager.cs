using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CoddleManager_Jamo : MonoBehaviour
{
    [Header("Main Parents")]
    public Transform coddleBackgroundParent;
    public Transform coddleTextParent;
    public TMP_InputField inputField;
    public GameObject warningText;

    [Header("Result Objects")]
    public GameObject successObject;
    public GameObject failObject;

    [Header("Settings")]
    private string targetWord;
    private string[] wordPool =
    {
        "생산",
        "공급",
        "평균",
        "분산",
        "인식",
        "철학"
    };

    private int maxAttempts = 5;

    private int currentAttempt = 0;
    private bool isGameOver = false;

    private List<List<Image>> baseLines = new List<List<Image>>();
    private List<List<TextMeshProUGUI>> textLines = new List<List<TextMeshProUGUI>>();

    private List<string> targetJamoList;

    void Start()
    {
        // 단어 랜덤 선택
        targetWord = wordPool[Random.Range(0, wordPool.Length)];
        targetJamoList = SplitToJamos(targetWord);

        TutorialResultManager.Instance.finalResult = "Global";
        Debug.Log("현재 단과대: " + TutorialResultManager.Instance.finalResult);

        // 배경 라인 자동 탐색
        foreach (Transform round in coddleBackgroundParent)
        {
            List<Image> bases = new List<Image>();
            foreach (Transform baseObj in round)
            {
                Image img = baseObj.GetComponent<Image>();
                if (img != null) bases.Add(img);
            }
            if (bases.Count > 0)
                baseLines.Add(bases);
        }

        // 텍스트 라인 자동 탐색
        foreach (Transform roundText in coddleTextParent)
        {
            List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
            foreach (Transform textObj in roundText)
            {
                TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
                if (tmp != null) texts.Add(tmp);
            }
            if (texts.Count > 0)
                textLines.Add(texts);
        }

        if (warningText != null) warningText.SetActive(false);
        if (successObject != null) successObject.SetActive(false);
        if (failObject != null) failObject.SetActive(false);

        inputField.onEndEdit.AddListener(OnSubmit);

        Debug.Log($"CoddleManager_Jamo 시작됨 / 정답: {targetWord} ({string.Join(",", targetJamoList)})");
    }

    void OnSubmit(string input)
    {
        if (inputField.wasCanceled) return;
        if (isGameOver) return;

        if (string.IsNullOrWhiteSpace(input)) return;

        input = input.Trim();
        var inputJamoList = SplitToJamos(input);

        // 자모 개수 다르면 경고
        if (inputJamoList.Count != targetJamoList.Count)
        {
            StartCoroutine(ShowWarning());
            inputField.text = "";
            inputField.ActivateInputField();
            return;
        }

        if (currentAttempt >= maxAttempts)
        {
            Debug.Log("모든 시도 완료");
            return;
        }

        var currentTextLine = textLines[currentAttempt];
        var currentBaseLine = baseLines[currentAttempt];

        string result = GetResultString_Jamo(inputJamoList);

        Debug.Log($"[{currentAttempt + 1}번째 시도] 결과: {result}");

        for (int i = 0; i < inputJamoList.Count; i++)
        {
            currentTextLine[i].text = inputJamoList[i];

            switch (result[i])
            {
                case 'O':
                    currentBaseLine[i].color = new Color(0.6f, 1f, 0.6f);
                    break;
                case '*':
                    currentBaseLine[i].color = new Color(1f, 1f, 0.6f);
                    break;
                case 'X':
                    currentBaseLine[i].color = new Color(1f, 0.6f, 0.6f);
                    break;
            }
        }

        // 정답
        if (result.Replace("*", "O") == new string('O', result.Length))
        {
            Debug.Log("정답입니다!");
            if (successObject != null) successObject.SetActive(true);
            EndGame();
            return;
        }

        currentAttempt++;

        // 5회 실패
        if (currentAttempt >= maxAttempts)
        {
            Debug.Log("5회 실패, 게임 종료");
            if (failObject != null) failObject.SetActive(true);
            EndGame();
            return;
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

    void EndGame()
    {
        isGameOver = true;
        inputField.interactable = false;
    }

    string GetResultString_Jamo(List<string> inputJamos)
    {
        int len = targetJamoList.Count;
        char[] result = new char[len];
        bool[] used = new bool[len];

        // 위치 + 글자 완전 일치
        for (int i = 0; i < len; i++)
        {
            if (inputJamos[i] == targetJamoList[i])
            {
                result[i] = 'O';
                used[i] = true;
            }
        }

        // 글자만 포함
        for (int i = 0; i < len; i++)
        {
            if (result[i] == 'O') continue;

            bool found = false;
            for (int j = 0; j < len; j++)
            {
                if (!used[j] && inputJamos[i] == targetJamoList[j])
                {
                    found = true;
                    used[j] = true;
                    break;
                }
            }

            result[i] = found ? '*' : 'X';
        }

        return new string(result);
    }

    List<string> SplitToJamos(string word)
    {
        List<string> jamos = new List<string>();

        string[] CHO = { "ㄱ","ㄲ","ㄴ","ㄷ","ㄸ","ㄹ","ㅁ","ㅂ","ㅃ","ㅅ","ㅆ","ㅇ","ㅈ","ㅉ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ" };
        string[] JUNG = { "ㅏ","ㅐ","ㅑ","ㅒ","ㅓ","ㅔ","ㅕ","ㅖ","ㅗ","ㅘ","ㅙ","ㅚ","ㅛ","ㅜ","ㅝ","ㅞ","ㅟ","ㅠ","ㅡ","ㅢ","ㅣ" };
        string[] JONG = { "", "ㄱ","ㄲ","ㄳ","ㄴ","ㄵ","ㄶ","ㄷ","ㄹ","ㄺ","ㄻ","ㄼ","ㄽ","ㄾ","ㄿ","ㅀ","ㅁ","ㅂ","ㅄ","ㅅ","ㅆ","ㅇ","ㅈ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ" };

        foreach (char c in word)
        {
            if (c < 0xAC00 || c > 0xD7A3)
            {
                jamos.Add(c.ToString());
                jamos.Add("");
                jamos.Add("");
                continue;
            }

            int code = c - 0xAC00;
            int cho = code / (21 * 28);
            int jung = (code % (21 * 28)) / 28;
            int jong = code % 28;

            jamos.Add(CHO[cho]);
            jamos.Add(JUNG[jung]);
            jamos.Add(JONG[jong]);
        }

        return jamos;
    }

    IEnumerator ShowWarning()
    {
        if (warningText == null) yield break;
        warningText.SetActive(true);
        yield return new WaitForSeconds(3f);
        warningText.SetActive(false);
    }
}
