using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string characterName;
    public Sprite characterSprite;
    [TextArea] public string text;
}


[CreateAssetMenu(fileName = "NewDialogData", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public DialogLine[] lines;
}
