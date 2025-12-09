using UnityEngine;

public class UIActivator : MonoBehaviour
{
    [Header("이 버튼을 누르면 활성화될 오브젝트들")]
    public GameObject[] activateTargets;

    [Header("이 버튼을 누르면 비활성화될 오브젝트들")]
    public GameObject[] deactivateTargets;

    public void OnClick()
    {
        // 활성화할 목록
        foreach (GameObject go in activateTargets)
            if (go != null) go.SetActive(true);

        // 비활성화할 목록
        foreach (GameObject go in deactivateTargets)
            if (go != null) go.SetActive(false);
    }
}
