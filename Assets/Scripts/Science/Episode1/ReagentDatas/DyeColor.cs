/*
 * 시약의 종류를 구분하기 위한 색상 Enum.
 * 각 시약 데이터(ReagentData)가 어떤 색 카테고리에 속하는지 정의한다.
 *
 * 사용 위치:
 *  - ReagentData.targetColor
 *  - ReagentCardDataHolder.target
 *  - GameManager에서 판정 시 비교용
 */

public enum DyeColor { Red = 0, Green = 1, Blue = 2 }
