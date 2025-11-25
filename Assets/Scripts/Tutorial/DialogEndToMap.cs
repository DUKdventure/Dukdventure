using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogEndToMap : MonoBehaviour
{
    public GameObject dialogPanel;   // DialogPanel 연결
    private bool moved = false;

    void Update()
    {
        if (!moved && dialogPanel.activeSelf == false)
        {
            moved = true;
            SceneManager.LoadScene("Map");
        }
    }
}
