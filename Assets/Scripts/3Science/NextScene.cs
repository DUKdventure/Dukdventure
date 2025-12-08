using UnityEngine;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    void Start()
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }
}
