using UnityEngine;

[CreateAssetMenu(menuName = "Sorting/Reagent", fileName = "Reagent_")]
public class ReagentData : ScriptableObject
{
    public string displayName;
    public Sprite sprite;
    public DyeColor targetColor;
}
