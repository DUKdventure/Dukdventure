/*
 * 각 시약의 정보를 저장하는 ScriptableObject.
 * 생성 방법:
 *  - Project 창 → Create → Sorting → Reagent
 *  - Inspector에서 Sprite와 TargetColor 지정
 */
using UnityEngine;

[CreateAssetMenu(menuName = "Sorting/Reagent", fileName = "Reagent_")]
public class ReagentData : ScriptableObject
{
    public Sprite sprite;
    public DyeColor targetColor;

    [Header("UI Size")]
    public Vector2 uiSize = new Vector2(322, 331);
    public Vector2 uiPivot = new Vector2(0.5f, 0f);

    public Vector2 uiOffset = Vector2.zero;
}
