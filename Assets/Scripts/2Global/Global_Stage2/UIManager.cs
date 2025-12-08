using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject clearUI;


    public GameObject greenUI;  // G_Book
    public GameObject yellowUI; // Y_Book
    public GameObject redUI;    // R_Book

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGreenBook() => greenUI.SetActive(true);
    public void ShowYellowBook() => yellowUI.SetActive(true);
    public void ShowRedBook() => redUI.SetActive(true);

    public void ShowClear()
{
    if (clearUI != null)
        clearUI.SetActive(true);
    }
}
