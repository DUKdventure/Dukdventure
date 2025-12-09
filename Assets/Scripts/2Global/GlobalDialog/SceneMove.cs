using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void Move(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
