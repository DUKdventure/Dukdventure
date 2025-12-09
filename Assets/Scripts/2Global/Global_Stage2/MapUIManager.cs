using UnityEngine;
using System.Collections.Generic;

public class MapUIManager : MonoBehaviour
{
    public static MapUIManager Instance;

    public List<GameObject> mapPoints; // 약도 14개 위치
    public GameObject greenIconPrefab;
    public GameObject yellowIconPrefab;
    public GameObject redIconPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SetMapIcons(List<int> idx)
    {
            Debug.Log("📌 SetMapIcons 실행됨");
        Instantiate(greenIconPrefab, mapPoints[idx[0]].transform);
        Instantiate(yellowIconPrefab, mapPoints[idx[1]].transform);
        Instantiate(redIconPrefab, mapPoints[idx[2]].transform);
    }
}
