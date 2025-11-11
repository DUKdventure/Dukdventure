using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    

    protected void Start()
    {
        if (dialogData != null && dialogData.lines.Length > 0)
            ShowSentence(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                //타이핑 중이면 전체 문장 바로 표시
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogData.lines[currentIndex].text;
                isTyping = false;
            }
            else
            {
                NextSentence();
            }
        }
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
        gameObject.SetActive(false);
    }

}
