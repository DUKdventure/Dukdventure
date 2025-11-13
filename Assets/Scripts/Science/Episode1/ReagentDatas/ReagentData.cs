/*
 * 각 시약의 정보를 저장하는 ScriptableObject.
 * 필드:
 *  - sprite : 시약 이미지
 *  - targetColor : 정답 색상 (DyeColor)
 *
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
}
