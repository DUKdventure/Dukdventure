using UnityEngine;

public class TutorialResultManager : MonoBehaviour
{
    public static TutorialResultManager Instance;

    public string finalResult; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   
        }
        else
        {
            Destroy(gameObject);            
        }
    }
}
