using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToChangeScene_Object : MonoBehaviour
{
    public string miniGameScene;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(miniGameScene);
    }
}
