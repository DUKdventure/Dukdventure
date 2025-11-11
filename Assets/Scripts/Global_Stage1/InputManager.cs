using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CoddleManager : MonoBehaviour
{
    public List<GameObject> lineObjects; 
    public TMP_InputField inputField;
    public int wordLength = 6;

    private int currentAttempt = 0;
    private string targetWord = "cookie"; 

    void Start()
    {
        foreach (var line in lineObjects)
        line.SetActive(false);

        inputField.onEndEdit.AddListener(OnSubmit);

        Debug.Log("CoddleManager 시작됨");
    }

    void OnSubmit(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return;
        input = input.Trim().ToLower();

        if (currentAttempt < lineObjects.Count)
        {
            var line = lineObjects[currentAttempt];
            line.SetActive(true);

            TextMeshProUGUI[] letters = line.GetComponentsInChildren<TextMeshProUGUI>(true);

            Debug.Log($"[{currentAttempt + 1}번째 시도] TMP 수: {letters.Length}");

            for (int i = 0; i < wordLength; i++)
            {
                if (i < input.Length)
                    letters[i].text = input[i].ToString();
                else
                    letters[i].text = "";
            }

            currentAttempt++;
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

}
