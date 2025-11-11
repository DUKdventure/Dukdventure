using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    [Header("UI References")]
    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogText;

    [Header("Data")]
    public DialogData dialogData;

    int currentIndex = 0;
    bool isTyping = false;
    Coroutine typingCoroutine;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;

    [Header("Controls")]
    public Button skipButton;
    public bool skipAll = true;

    [Header("Scene Flow")]
    public string nextSceneName;

    bool isLoading = false;

    protected void Start()
    {
        if (skipButton)
            skipButton.onClick.AddListener(OnSkipClicked);

        if (dialogData != null && dialogData.lines.Length > 0)
            ShowSentence(0);
    }

    void Update()
    {
        if (isLoading) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                CompleteTypingCurrentLine();
            }
            else
            {
                NextSentence();
            }
        }
    }

    public void OnSkipClicked()
    {
        if (isLoading) return;

        if (isTyping)
        {
            CompleteTypingCurrentLine();
            return;
        }

        if (skipAll)
        {
            EndDialog();
        }
        else
        {
            NextSentence();
        }
    }

    void CompleteTypingCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogText.text = dialogData.lines[currentIndex].text;
        isTyping = false;
    }

    protected void ShowSentence(int index)
    {
        if (index >= dialogData.lines.Length)
        {
            EndDialog();
            return;
        }

        var line = dialogData.lines[index];
        characterImage.sprite = line.characterSprite;
        characterNameText.text = line.characterName;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(line.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in sentence)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    void NextSentence()
    {
        currentIndex++;
        if (currentIndex < dialogData.lines.Length)
            ShowSentence(currentIndex);
        else
            EndDialog();
    }

    void EndDialog()
    {
        Debug.Log("대화 종료");

        isLoading = true;

        gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("[Dialog] SceneLoader.Instance가 없어 바로 씬 로드합니다.");
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }    
}
