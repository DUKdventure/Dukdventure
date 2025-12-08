using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string characterName;
    public Sprite characterSprite;
    [TextArea] public string text;

    public Vector2 imageSize = new Vector2(400, 400);

}


[CreateAssetMenu(fileName = "NewDialogData", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public DialogLine[] lines;
}
