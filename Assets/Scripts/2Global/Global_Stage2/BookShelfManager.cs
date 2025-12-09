using UnityEngine;
using System.Collections.Generic;

public class BookShelfManager : MonoBehaviour
{
    public static BookShelfManager Instance;

    public List<BookShelf> shelves;       // BookCase 1~14
    public List<int> selectedShelves;     // 랜덤 선택 3개

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {       
        Debug.Log("BookShelfManager START 실행됨");
        SelectRandomShelves();
    }

    void SelectRandomShelves()
    {   
        List<int> temp = new List<int>();
        for (int i = 0; i < shelves.Count; i++)
            temp.Add(i);

        selectedShelves = new List<int>();

        // 3개 랜덤 선택
        for (int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, temp.Count);
            int shelfIndex = temp[r];
            temp.RemoveAt(r);

            selectedShelves.Add(shelfIndex);
        }

        // 단계 부여
        shelves[selectedShelves[0]].SetBookStage(1); // 초록
        shelves[selectedShelves[1]].SetBookStage(2); // 노랑
        shelves[selectedShelves[2]].SetBookStage(3); // 빨강

        // 약도 UI에 전달
        MapUIManager.Instance.SetMapIcons(selectedShelves);
    }
}
