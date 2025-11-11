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
    private string targetWord = "덕성";

    private int currentAttempt = 0;
    private bool isGameOver = false;

    private List<List<Image>> baseLines = new List<List<Image>>();
    private List<List<TextMeshProUGUI>> textLines = new List<List<TextMeshProUGUI>>();

    private List<string> targetJamoList; // 자모 분리된 정답 리스트

    void Start()
    {
        targetJamoList = SplitToJamos(targetWord); 
        // 하위 오브젝트 자동 탐색
        foreach (Transform round in coddleBackgroundParent)
        {
            List<Image> bases = new List<Image>();
            foreach (Transform baseObj in round)
            {
                Image img = baseObj.GetComponent<Image>();
                if (img != null)
                    bases.Add(img);
            }
            if (bases.Count > 0)
                baseLines.Add(bases);
        }

        foreach (Transform roundText in coddleTextParent)
        {
            List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
            foreach (Transform textObj in roundText)
            {
                TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                    texts.Add(tmp);
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

        if (inputJamoList.Count != targetJamoList.Count)
        {
            StartCoroutine(ShowWarning());
            inputField.text = "";
            inputField.ActivateInputField();
            return;
        }

        if (currentAttempt >= textLines.Count)
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
                    currentBaseLine[i].color = new Color(0.6f, 1f, 0.6f); // 초록
                    break;
                case '*':
                    currentBaseLine[i].color = new Color(1f, 1f, 0.6f);   // 노랑
                    break;
                case 'X':
                    currentBaseLine[i].color = new Color(1f, 0.6f, 0.6f); // 빨강
                    break;
            }
        }

        // 정답 맞춘 경우
        if (result.Replace("*", "O") == new string('O', result.Length))
        {
            Debug.Log("✅ 정답입니다! 게임 종료");
            if (successObject != null) successObject.SetActive(true);
            EndGame();
            return;
        }

        currentAttempt++;

        if (currentAttempt >= 3)
        {
            Debug.Log("❌ 3회 실패, 게임 종료");
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

    // 자모 단위 비교 로직
    string GetResultString_Jamo(List<string> inputJamos)
    {
        int len = targetJamoList.Count;
        char[] result = new char[len];
        bool[] used = new bool[len];

        // 위치+글자 일치
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

    // 한글을 자모 단위로 분리
    // 한글 자모 3개(초/중/종) 고정 분리 버전
    List<string> SplitToJamos(string word)
    {
        List<string> jamos = new List<string>();

        string[] CHO = { "ㄱ","ㄲ","ㄴ","ㄷ","ㄸ","ㄹ","ㅁ","ㅂ","ㅃ","ㅅ","ㅆ","ㅇ","ㅈ","ㅉ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ" };
        string[] JUNG = { "ㅏ","ㅐ","ㅑ","ㅒ","ㅓ","ㅔ","ㅕ","ㅖ","ㅗ","ㅘ","ㅙ","ㅚ","ㅛ","ㅜ","ㅝ","ㅞ","ㅟ","ㅠ","ㅡ","ㅢ","ㅣ" };
        string[] JONG = { "", "ㄱ","ㄲ","ㄳ","ㄴ","ㄵ","ㄶ","ㄷ","ㄹ","ㄺ","ㄻ","ㄼ","ㄽ","ㄾ","ㄿ","ㅀ","ㅁ","ㅂ","ㅄ","ㅅ","ㅆ","ㅇ","ㅈ","ㅊ","ㅋ","ㅌ","ㅍ","ㅎ" };

        foreach (char c in word)
        {
            // 한글 유니코드 범위 확인
            if (c < 0xAC00 || c > 0xD7A3)
            {
                // 한글이 아니면 그냥 세 칸 맞춰 넣기
                jamos.Add(c.ToString());
                jamos.Add("");
                jamos.Add("");
                continue;
            }

            int code = c - 0xAC00;
            int cho = code / (21 * 28);
            int jung = (code % (21 * 28)) / 28;
            int jong = code % 28;

            jamos.Add(CHO[cho]);          // 초성
            jamos.Add(JUNG[jung]);        // 중성
            jamos.Add(JONG[jong]);        // 종성 (없으면 "" 자동)
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
