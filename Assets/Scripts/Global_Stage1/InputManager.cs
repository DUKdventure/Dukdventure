using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CoddleManager : MonoBehaviour
{
    [Header("Main Parents")]
    public Transform coddleBackgroundParent; // CoddleBackground
    public Transform coddleTextParent;       // Coddle_Text
    public TMP_InputField inputField;
    public GameObject warningText;

    [Header("Result Objects")]
    public GameObject successObject;
    public GameObject failObject;

    [Header("Settings")]
    public int wordLength = 6;
    private string targetWord = "cookie";

    private int currentAttempt = 0;
    private bool isGameOver = false;

    private List<List<Image>> baseLines = new List<List<Image>>();
    private List<List<TextMeshProUGUI>> textLines = new List<List<TextMeshProUGUI>>();

    void Start()
    {
        // 하위 자동 탐색
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
        Debug.Log($"CoddleManager 시작됨 / 라운드 {textLines.Count}개 감지됨");
    }

    void OnSubmit(string input)
    {
        if (inputField.wasCanceled) return;
        if (isGameOver) return;
        if (string.IsNullOrWhiteSpace(input)) return;
        input = input.Trim().ToLower();

        if (input.Length != wordLength)
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

        string result = GetResultString(input);
        Debug.Log($"[{currentAttempt + 1}번째 시도] 결과: {result}");

        for (int i = 0; i < wordLength; i++)
        {
            currentTextLine[i].text = input[i].ToString().ToUpper();

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
        if (input == targetWord)
        {
            Debug.Log("✅ 정답입니다! 게임 종료");
            if (successObject != null) successObject.SetActive(true);
            EndGame();
            return;
        }

        currentAttempt++;

        // 3회 시도 후 실패 처리
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

    string GetResultString(string input)
    {
        char[] result = new char[wordLength];
        bool[] targetUsed = new bool[wordLength];

        // 자리+글자 일치
        for (int i = 0; i < wordLength; i++)
        {
            if (input[i] == targetWord[i])
            {
                result[i] = 'O';
                targetUsed[i] = true;
            }
        }

        // 글자만 포함
        for (int i = 0; i < wordLength; i++)
        {
            if (result[i] == 'O') continue;
            bool found = false;
            for (int j = 0; j < wordLength; j++)
            {
                if (!targetUsed[j] && input[i] == targetWord[j])
                {
                    found = true;
                    targetUsed[j] = true;
                    break;
                }
            }
            result[i] = found ? '*' : 'X';
        }

        return new string(result);
    }

    IEnumerator ShowWarning()
    {
        if (warningText == null) yield break;
        warningText.SetActive(true);
        yield return new WaitForSeconds(3f);
        warningText.SetActive(false);
    }
}
