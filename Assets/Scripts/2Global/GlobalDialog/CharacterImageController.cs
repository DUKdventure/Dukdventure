using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogVisualUpdater : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public Image characterImage;

    public void Apply(DialogLine line)
    {   
            Debug.Log("🔍 Apply() 호출됨! 받은 캐릭터 이름 = " + line.characterName);

        if (line == null) return;

        // 이름 적용
        characterNameText.text = line.characterName;

        // 이미지 적용
        if (line.characterSprite != null)
            characterImage.sprite = line.characterSprite;

        // 크기/활성화 처리
        switch (line.characterName)
        {
            case "덕새":
                characterImage.gameObject.SetActive(false);
                break;

            case "복단":
                characterImage.gameObject.SetActive(true);
                characterImage.rectTransform.sizeDelta = new Vector2(500, 500);
                break;

            case "교수":
                characterImage.gameObject.SetActive(true);
                characterImage.rectTransform.sizeDelta = new Vector2(450, 650);
                break;

            default:
                characterImage.gameObject.SetActive(true);
                characterImage.rectTransform.sizeDelta = new Vector2(400, 400);
                break;
        }
    }
}
